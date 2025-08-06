using System;
using System.ComponentModel.DataAnnotations;

namespace CSRWebsiteMVC.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Profile Picture")]
    public IFormFile? ProfileImage { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }


}
