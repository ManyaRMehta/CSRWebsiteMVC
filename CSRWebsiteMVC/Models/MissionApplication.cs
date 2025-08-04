using System;

namespace CSRWebsiteMVC.Models;

public class MissionApplication
{
    public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int MissionId { get; set; }
        public Mission Mission { get; set; }

        public string Status { get; set; } // "Pending", "Accepted", "Rejected"

        public DateTime AppliedOn { get; set; } = DateTime.UtcNow;

}
