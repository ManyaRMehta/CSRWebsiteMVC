using System.ComponentModel.DataAnnotations;

namespace CSRWebsiteMVC.Models
{
    public class MissionTheme
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Theme Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
