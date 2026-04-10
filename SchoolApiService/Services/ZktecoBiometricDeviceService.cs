using System.Runtime.InteropServices;
using Microsoft.CSharp.RuntimeBinder;
using SchoolApiService.ViewModels;

namespace SchoolApiService.Services
{
    public class ZktecoBiometricDeviceService : IBiometricDeviceService
    {
        public async Task<IReadOnlyList<BiometricLogEntry>> FetchLogsAsync(
            ZktecoMachineFetchRequest request,
            CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => FetchLogsInternal(request), cancellationToken);
        }

        private static IReadOnlyList<BiometricLogEntry> FetchLogsInternal(ZktecoMachineFetchRequest request)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new InvalidOperationException("ZKTeco SDK access is only available on Windows hosts.");
            }

            var progIdType = Type.GetTypeFromProgID("zkemkeeper.CZKEM");
            if (progIdType == null)
            {
                throw new InvalidOperationException(
                    "ZKTeco SDK is not installed/registered. Please register zkemkeeper.dll on the API server.");
            }

            dynamic device = Activator.CreateInstance(progIdType)
                ?? throw new InvalidOperationException("Unable to initialize ZKTeco device SDK.");

            var machineNo = request.MachineNo <= 0 ? 1 : request.MachineNo;
            var logs = new List<BiometricLogEntry>();

            try
            {
                var connected = device.Connect_Net(request.IP, request.Port);
                if (!connected)
                {
                    throw new InvalidOperationException($"Failed to connect to device {request.IP}:{request.Port}.");
                }

                if (request.CommKey > 0)
                {
                    try
                    {
                        device.SetCommPassword(request.CommKey);
                    }
                    catch
                    {
                        // Some SDK variants do not expose this method. Safe to ignore.
                    }
                }

                try
                {
                    device.EnableDevice(machineNo, false);
                }
                catch
                {
                    // Optional; continue if not supported.
                }

                var readStarted = false;
                try
                {
                    readStarted = device.ReadGeneralLogData(machineNo);
                }
                catch
                {
                    // ignore and fallback to direct loop below
                }

                if (!readStarted)
                {
                    // If device has no new logs, ReadGeneralLogData can return false.
                    // We still attempt to iterate once via GetGeneralLogData methods.
                }

                BiometricLogEntry entry;
                while (TryReadLog(device, machineNo, out entry))
                {
                    logs.Add(entry);
                }
            }
            finally
            {
                try
                {
                    device.EnableDevice(machineNo, true);
                }
                catch
                {
                    // ignore
                }

                try
                {
                    device.Disconnect();
                }
                catch
                {
                    // ignore
                }
            }

            return logs;
        }

        private static bool TryReadLog(dynamic device, int machineNo, out BiometricLogEntry entry)
        {
            entry = new BiometricLogEntry();

            // SSR mode (string enroll number) - modern devices.
            try
            {
                string enrollNumber;
                int verifyMode;
                int inOutMode;
                int year;
                int month;
                int day;
                int hour;
                int minute;
                int second;
                int workCode;

                var ok = device.SSR_GetGeneralLogData(
                    machineNo,
                    out enrollNumber,
                    out verifyMode,
                    out inOutMode,
                    out year,
                    out month,
                    out day,
                    out hour,
                    out minute,
                    out second,
                    out workCode);

                if (!ok)
                {
                    return false;
                }

                entry = new BiometricLogEntry
                {
                    EnrollNumber = enrollNumber ?? string.Empty,
                    PunchTime = new DateTime(year, month, day, hour, minute, second),
                    VerifyMode = verifyMode,
                    InOutMode = inOutMode,
                    WorkCode = workCode
                };

                return true;
            }
            catch (RuntimeBinderException)
            {
                // fall through to legacy mode
            }

            // Legacy mode (numeric enroll number).
            try
            {
                int enrollNumber;
                int verifyMode;
                int inOutMode;
                int year;
                int month;
                int day;
                int hour;
                int minute;
                int second;
                int workCode;

                var ok = device.GetGeneralLogData(
                    machineNo,
                    out enrollNumber,
                    out verifyMode,
                    out inOutMode,
                    out year,
                    out month,
                    out day,
                    out hour,
                    out minute,
                    out second,
                    out workCode);

                if (!ok)
                {
                    return false;
                }

                entry = new BiometricLogEntry
                {
                    EnrollNumber = enrollNumber.ToString(),
                    PunchTime = new DateTime(year, month, day, hour, minute, second),
                    VerifyMode = verifyMode,
                    InOutMode = inOutMode,
                    WorkCode = workCode
                };

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
