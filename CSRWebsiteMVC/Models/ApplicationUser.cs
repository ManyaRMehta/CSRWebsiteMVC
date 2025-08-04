using System;
using Microsoft.AspNetCore.Identity;

namespace CSRWebsiteMVC.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public string? PhotoPath { get; set; }
    public string? EmployeeId { get; set; }
    public string? Department { get; set; }
    public string? Title { get; set; }

    public ICollection<MissionApplication> Applications { get; set; }

}
