using System.ComponentModel.DataAnnotations;

namespace SchoolApiService.ViewModels
{
    public class ZktecoMachineFetchRequest
    {
        public int MachineId { get; set; }

        [Required]
        public int MachineNo { get; set; }

        public string? MachineName { get; set; }

        [Required]
        public string IP { get; set; } = string.Empty;

        [Required]
        public int Port { get; set; } = 4370;

        public int CommKey { get; set; }

        public DateTime? Date { get; set; }
    }
}
