using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSRWebsiteMVC.Models;

public class Mission
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Display(Name = "End Date")]
    [DataType(DataType.Date)]

    public DateTime EndDate { get; set; }
    [Display(Name = "Total Seats")]

    public int TotalSeats { get; set; }
    [Display(Name = "Mission Theme")]
    public int MissionThemeId { get; set; }
    public MissionTheme? MissionTheme { get; set; }

    
    public List<MissionSkillAssignment> MissionSkillAssignments { get; set; } = new();
    [NotMapped]
public List<int> SelectedSkillIds { get; set; } = new();



    public List<MissionApplication> Applications { get; set; } = new();

}
