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
    public ReportPriority Priority { get; set; } = ReportPriority.Normal;

// User relationships
    public int? SubmittedByUserId { get; set; }
    public User? SubmittedByUser { get; set; }

    public int? ReviewedByUserId { get; set; }
    public User? ReviewedByUser { get; set; }

// Admin notes
    public string? ModeratorNotes { get; set; }
    public string? InternalNotes { get; set; }

// Sensitivity flags
    public bool IsHighPriority { get; set; } = false;
    public bool RequiresGardaAttention { get; set; } = false;

// Add these timestamps
    public DateTime? DateReviewed { get; set; }
    public DateTime? DateResolved { get; set; }
    
    public enum ReportPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Critical = 3
    }
}