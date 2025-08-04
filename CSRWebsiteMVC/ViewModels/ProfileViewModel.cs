using System;
using CSRWebsiteMVC.Models;

namespace CSRWebsiteMVC.ViewModels;

public class ProfileViewModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public List<MissionApplication> Applications { get; set; } = new();

}
