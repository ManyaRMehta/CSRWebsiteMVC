using CSRWebsiteMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSRWebsiteMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
            private readonly ApplicationDbContext _context;

            public AdminDashboardController(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Index()
            {
                var totalUsers = await _context.Users.CountAsync();
                var totalMissions = await _context.Missions.CountAsync();
                var totalApplications = await _context.MissionApplications.CountAsync();
                var topMission = await _context.MissionApplications
                    .GroupBy(a => a.MissionId)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new
                    {
                        MissionTitle = g.First().Mission.Title,
                        Applications = g.Count()
                    })
                    .FirstOrDefaultAsync();

                ViewBag.TotalUsers = totalUsers;
                ViewBag.TotalMissions = totalMissions;
                ViewBag.TotalApplications = totalApplications;
                ViewBag.TopMissionTitle = topMission?.MissionTitle ?? "N/A";
                ViewBag.TopMissionCount = topMission?.Applications ?? 0;

                return View();
            }
        

    }
}
