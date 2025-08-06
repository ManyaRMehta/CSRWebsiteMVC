using CSRWebsiteMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSRWebsiteMVC.Controllers;

[Authorize(Roles = "Admin")]
public class AdminApplicationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminApplicationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var applications = await _context.MissionApplications
            .Include(a => a.Mission)
            .Include(a => a.User)
            .ToListAsync();

        return View(applications);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id)
    {
        var application = await _context.MissionApplications.FindAsync(id);
        if (application != null)
        {
            application.Status = "Approved";
            TempData["ToastMessage"] = "Application approved successfully!";
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id)
    {
        var application = await _context.MissionApplications.FindAsync(id);
        if (application != null)
        {
            application.Status = "Rejected";
            TempData["ToastMessage"] = "Application rejected successfully!";
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}
