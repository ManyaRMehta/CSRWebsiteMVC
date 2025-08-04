using System;

namespace CSRWebsiteMVC.Models;

public class MissionSkillAssignment
{
    public int MissionId { get; set; }
    public Mission Mission { get; set; }

    public int MissionSkillId { get; set; }
    public MissionSkill MissionSkill { get; set; }

}
