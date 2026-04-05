namespace SafeVoice.Models;

public class AdminDashboardViewModel
{
    public int TotalReports { get; set; }
    public int PendingReports { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveSupportLocations { get; set; }
    public List<Report> RecentReports { get; set; } = new();
}