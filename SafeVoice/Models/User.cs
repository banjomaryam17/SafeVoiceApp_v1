using System.ComponentModel.DataAnnotations;

namespace SafeVoice.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Username { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    [Required]
    public UserRole Role { get; set; } = UserRole.Regular;
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BadgeNumber { get; set; } // For Garda
    public string? Department { get; set; }  // For SocialServices/Institutions
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? LastLogin { get; set; }
    
    // Navigation properties
    public ICollection<Report> Reports { get; set; } = new List<Report>();
}

public enum UserRole
{
    Regular = 0,        // Can submit reports only
    Moderator = 1,      // Internal staff - first review, can flag for escalation  
    Garda = 2,         // An Garda Síochána - full access, can take action
    SocialServices = 3, // HSE/Tusla - full access, child protection authority
    SuperAdmin = 4      // System administrator - full platform control
}

public static class UserRoleExtensions
{
    public static string GetDisplayName(this UserRole role)
    {
        return role switch
        {
            UserRole.Regular => "Regular User",
            UserRole.Moderator => "Moderator",
            UserRole.Garda => "An Garda Síochána",
            UserRole.SocialServices => "Social Services",
            UserRole.SuperAdmin => "Super Administrator",
            _ => role.ToString()
        };
    }
    
    public static bool CanViewAllReports(this UserRole role)
    {
        return role >= UserRole.Moderator;
    }
    
    public static bool CanViewSensitiveDetails(this UserRole role)
    {
        return role >= UserRole.Garda;
    }
    
    public static bool CanManageReports(this UserRole role)
    {
        return role >= UserRole.Garda;
    }
    
    public static bool CanManageUsers(this UserRole role)
    {
        return role == UserRole.SuperAdmin;
    }
}
