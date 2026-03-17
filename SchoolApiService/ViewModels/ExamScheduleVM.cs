namespace SchoolApiService.ViewModels
{
    public class ExamScheduleVM
    {
        public int ExamScheduleId { get; set; }
        public string? ExamScheduleName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ExamYear { get; set; }
        public IEnumerable<ExamScheduleStandardForExamScheduleVM>? ExamScheduleStandards { get; set; } = [];
    }
}
