using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CSRWebsiteMVC.ViewModels;

public class EditProfileViewModel
{
    [Required]
    public string FullName { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    public string? ExistingPhotoPath { get; set; }

    public IFormFile? NewProfileImage { get; set; }
        

}
