using CSRWebsiteMVC.Data;
using CSRWebsiteMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CSRWebsiteMVC.Controllers
{
    public class MissionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public MissionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var missions = await _context.Missions
                .Include(m => m.MissionTheme)
                .Include(m => m.MissionSkillAssignments)
                    .ThenInclude(msa => msa.MissionSkill)
                .ToListAsync();

            // Check if user is logged in and is a User (not Admin)
            var userId = _userManager.GetUserId(User);
            var userApplications = new HashSet<int>();

            if (User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                userApplications = _context.MissionApplications
                    .Where(ma => ma.UserId == userId)
                    .Select(ma => ma.MissionId)
                    .ToHashSet();
            }

            ViewBag.UserApplications = userApplications;

            return View(missions);
        }



        // POST: Missions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Mission mission)
        {
            mission.StartDate = DateTime.SpecifyKind(mission.StartDate, DateTimeKind.Utc);
            mission.EndDate = DateTime.SpecifyKind(mission.EndDate, DateTimeKind.Utc);

            ModelState.Remove("MissionTheme");
            if (!ModelState.IsValid)
            {


                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"❌ Field: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }

                ViewData["MissionThemeId"] = new SelectList(_context.MissionThemes, "Id", "Title", mission.MissionThemeId);
                return View(mission);
            }
            foreach (var skillId in mission.SelectedSkillIds)
            {
                mission.MissionSkillAssignments.Add(new MissionSkillAssignment
                {
                    MissionSkillId = skillId
                });
            }

            _context.Add(mission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Missions/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var mission = await _context.Missions
                .Include(m => m.MissionSkillAssignments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mission == null) return NotFound();

            ViewData["MissionThemeId"] = new SelectList(_context.MissionThemes, "Id", "Title", mission.MissionThemeId);

            var allSkills = await _context.MissionSkills.ToListAsync();
            var selectedSkillIds = mission.MissionSkillAssignments.Select(ms => ms.MissionSkillId).ToList();

            ViewBag.AllSkills = new MultiSelectList(allSkills, "Id", "SkillName", selectedSkillIds);

            return View(mission);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Mission mission, int[] selectedSkills)
        {
            if (id != mission.Id) return NotFound();

            if (ModelState.IsValid)
            {
                mission.StartDate = DateTime.SpecifyKind(mission.StartDate, DateTimeKind.Utc);
                mission.EndDate = DateTime.SpecifyKind(mission.EndDate, DateTimeKind.Utc);

                // Get the existing mission from DB with skills
                var missionToUpdate = await _context.Missions
                    .Include(m => m.MissionSkillAssignments)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (missionToUpdate == null) return NotFound();

                // Update basic fields
                missionToUpdate.Title = mission.Title;
                missionToUpdate.Description = mission.Description;
                missionToUpdate.Location = mission.Location;
                missionToUpdate.TotalSeats = mission.TotalSeats;
                missionToUpdate.StartDate = mission.StartDate;
                missionToUpdate.EndDate = mission.EndDate;
                missionToUpdate.MissionThemeId = mission.MissionThemeId;

                // Clear existing skills and reassign
                missionToUpdate.MissionSkillAssignments.Clear();

                foreach (var skillId in selectedSkills)
                {
                    missionToUpdate.MissionSkillAssignments.Add(new MissionSkillAssignment
                    {
                        MissionId = mission.Id,
                        MissionSkillId = skillId
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MissionThemeId"] = new SelectList(_context.MissionThemes, "Id", "Title", mission.MissionThemeId);

            var allSkills = await _context.MissionSkills.ToListAsync();
            ViewBag.AllSkills = new MultiSelectList(allSkills, "Id", "SkillName", selectedSkills);

            return View(mission);
        }




        // GET: Missions/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var mission = await _context.Missions
                .Include(m => m.MissionTheme)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mission == null) return NotFound();

            return View(mission);
        }



        // POST: Missions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mission = await _context.Missions.FindAsync(id);
            _context.Missions.Remove(mission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Apply(int missionId)
        {
            var userId = _userManager.GetUserId(User);

            var alreadyApplied = await _context.MissionApplications
                .AnyAsync(a => a.MissionId == missionId && a.UserId == userId);

            if (!alreadyApplied)
            {
                var application = new MissionApplication
                {
                    MissionId = missionId,
                    UserId = userId,
                    Status = "Pending",
                    AppliedOn = DateTime.UtcNow
                };

                _context.MissionApplications.Add(application);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var mission = await _context.Missions
                .Include(m => m.MissionTheme)
                .Include(m => m.MissionSkillAssignments)
                    .ThenInclude(msa => msa.MissionSkill)
                .Include(m => m.Applications)
                    .ThenInclude(app => app.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mission == null) return NotFound();

            return View(mission);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> ViewDetails(int? id)
        {
            if (id == null) return NotFound();

            var mission = await _context.Missions
                .Include(m => m.MissionTheme)
                .Include(m => m.MissionSkillAssignments)
                    .ThenInclude(msa => msa.MissionSkill)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mission == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            // Check if the user has applied to this mission
            var hasApplied = await _context.MissionApplications
                .AnyAsync(a => a.MissionId == mission.Id && a.UserId == userId);

            ViewBag.HasApplied = hasApplied;

            return View("MissionDetailsUser", mission);
        }





    }
}
