namespace SchoolApp.Models.DataModels
{
    public class FeatureRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public string? UserName { get; set; }
    }
}
