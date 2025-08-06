using System;
using System.ComponentModel.DataAnnotations;

namespace CSRWebsiteMVC.ViewModels;

public class CreateAdminViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; }

    [Required]
    [Phone]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
        

}
