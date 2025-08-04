using System;
using System.ComponentModel.DataAnnotations;

namespace CSRWebsiteMVC.Models;

public class MissionSkill
{
    public int Id { get; set; }

    [Required]
    public string SkillName { get; set; }

    public string? Description { get; set; }
    public List<Mission> Missions { get; set; } = new();
    public List<MissionSkillAssignment> MissionSkillAssignments { get; set; } = new();



}
