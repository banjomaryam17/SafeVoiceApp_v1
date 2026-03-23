using System.ComponentModel.DataAnnotations;

namespace SafeVoice.Models;

public class Report
{
    public int Id { get; set; }
    [Required]
    public  string Description { get; set; }
    [Required]
    public ReportingAs ReportingAs { get; set; }
    
    public string? VictimName { get; set; }
    public int? VictimAge { get; set; }
    
    public string? RelationshipToVictim{get;set;}
    public string? ReporterName { get; set; }
    public string? ReporterContact { get; set; }
    [Required]
    public string Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    public ReportStatus Status { get; set; } = ReportStatus.Pending;

    public DateTime DateSubmitted { get; set; } = DateTime.Now;
    
    
}