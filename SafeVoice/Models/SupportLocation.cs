using System.ComponentModel.DataAnnotations;

namespace SafeVoice.Models;

public class SupportLocation
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public SupportLocationType Type { get; set; }

    [Required]
    [StringLength(200)]
    public string Address { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Url]
    public string? Website { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    // Operating hours
    public string? OpeningHours { get; set; }

    // Additional info
    public string? Description { get; set; }
    public string? Services { get; set; } // What services they provide
    public bool Is24Hour { get; set; } = false;
    public bool IsEmergency { get; set; } = false;

    // Administrative
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // For Garda stations
    public string? StationCode { get; set; }
    public string? District { get; set; }
}

public enum SupportLocationType
{
    GardaStation = 0,
    Tusla = 1,           // Child and Family Agency
    HSE = 2,             // Health Service Executive
    SocialServices = 3,   // General social services
    Counselling = 4,      // Counselling services
    Helpline = 5,         // Phone support
    Hospital = 6,         // Emergency medical
    LegalAid = 7,         // Legal support
    CharityOrg = 8        // NGO/Charity support
}

public static class SupportLocationExtensions
{
    public static string GetDisplayName(this SupportLocationType type)
    {
        return type switch
        {
            SupportLocationType.GardaStation => "Garda Station",
            SupportLocationType.Tusla => "Tusla Office",
            SupportLocationType.HSE => "HSE Service",
            SupportLocationType.SocialServices => "Social Services",
            SupportLocationType.Counselling => "Counselling Service",
            SupportLocationType.Helpline => "Support Helpline",
            SupportLocationType.Hospital => "Hospital",
            SupportLocationType.LegalAid => "Legal Aid",
            SupportLocationType.CharityOrg => "Support Organisation",
            _ => type.ToString()
        };
    }

    public static bool IsEmergencyService(this SupportLocationType type)
    {
        return type == SupportLocationType.GardaStation || 
               type == SupportLocationType.Hospital;
    }

    public static string GetIconClass(this SupportLocationType type)
    {
        return type switch
        {
            SupportLocationType.GardaStation => "🚔",
            SupportLocationType.Tusla => "🏢",
            SupportLocationType.HSE => "🏥",
            SupportLocationType.SocialServices => "🤝",
            SupportLocationType.Counselling => "💭",
            SupportLocationType.Helpline => "📞",
            SupportLocationType.Hospital => "🏥",
            SupportLocationType.LegalAid => "⚖️",
            SupportLocationType.CharityOrg => "💙",
            _ => "📍"
        };
    }
}
