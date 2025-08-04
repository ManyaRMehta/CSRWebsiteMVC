using System;
using CSRWebsiteMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSRWebsiteMVC.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Mission> Missions { get; set; }
    public DbSet<MissionTheme> MissionThemes { get; set; }
    public DbSet<MissionSkill> MissionSkills { get; set; }
    public DbSet<MissionApplication> MissionApplications { get; set; }

    
    public DbSet<MissionSkillAssignment> MissionSkillAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ✅ Explicit many-to-many config
        modelBuilder.Entity<MissionSkillAssignment>()
            .HasKey(msa => new { msa.MissionId, msa.MissionSkillId });

        modelBuilder.Entity<MissionSkillAssignment>()
            .HasOne(msa => msa.Mission)
            .WithMany(m => m.MissionSkillAssignments)
            .HasForeignKey(msa => msa.MissionId);

        modelBuilder.Entity<MissionSkillAssignment>()
            .HasOne(msa => msa.MissionSkill)
            .WithMany(s => s.MissionSkillAssignments)
            .HasForeignKey(msa => msa.MissionSkillId);
    }
}
