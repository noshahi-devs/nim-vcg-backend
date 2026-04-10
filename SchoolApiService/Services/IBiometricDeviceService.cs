using SchoolApiService.ViewModels;

namespace SchoolApiService.Services
{
    public interface IBiometricDeviceService
    {
        Task<IReadOnlyList<BiometricLogEntry>> FetchLogsAsync(ZktecoMachineFetchRequest request, CancellationToken cancellationToken = default);
    }

    public sealed class BiometricLogEntry
    {
        public string EnrollNumber { get; set; } = string.Empty;
        public DateTime PunchTime { get; set; }
        public int VerifyMode { get; set; }
        public int InOutMode { get; set; }
        public int WorkCode { get; set; }
    }
}
