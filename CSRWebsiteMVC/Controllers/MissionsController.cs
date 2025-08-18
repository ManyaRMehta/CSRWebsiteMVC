using CSRWebsiteMVC.Data;
using CSRWebsiteMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
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
        [Authorize]

        public async Task<IActionResult> Index(string searchString, int? themeId, List<int> selectedSkillIds)
        {
            var missionsQuery = _context.Missions
                .Include(m => m.MissionTheme)
                .Include(m => m.MissionSkillAssignments)
                    .ThenInclude(msa => msa.MissionSkill)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                missionsQuery = missionsQuery.Where(m => m.Title.Contains(searchString)|| m.Description.Contains(searchString));
            }
            if (themeId.HasValue)
            {
                missionsQuery = missionsQuery.Where(m => m.MissionThemeId == themeId.Value);
            }

            if (selectedSkillIds != null && selectedSkillIds.Any())
            {
                missionsQuery = missionsQuery.Where(m => m.MissionSkillAssignments.Any(msa => selectedSkillIds.Contains(msa.MissionSkillId)));
            }
            var missions = await missionsQuery.ToListAsync();

            ViewBag.Themes = await _context.MissionThemes.ToListAsync();
            ViewBag.Skills = await _context.MissionSkills.ToListAsync();
            ViewBag.SelectedThemeId = themeId;
            ViewBag.SelectedSkillIds = selectedSkillIds ?? new List<int>();
            ViewBag.SearchString = searchString;

            
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["MissionThemeId"] = new SelectList(_context.MissionThemes, "Id", "Title");

            // 🔧 This line was missing and caused the crash
            ViewBag.Skills = _context.MissionSkills.ToList();

            return View(new Mission
            {
                SelectedSkillIds = new List<int>() // Optional but good to initialize
            });
        }






        // POST: Missions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Mission mission, IFormFile? imageFile)
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

            // Handle image upload if provided
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                mission.ImagePath = "/uploads/" + uniqueFileName;
            }

            // Save selected skills
            foreach (var skillId in mission.SelectedSkillIds)
            {
                mission.MissionSkillAssignments.Add(new MissionSkillAssignment
                {
                    MissionSkillId = skillId
                });
            }

            _context.Add(mission);
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Mission created successfully!";
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
        public async Task<IActionResult> Edit(int id, Mission mission, int[] selectedSkills, IFormFile? imageFile)
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

                // Handle image upload if a new file is provided
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    missionToUpdate.ImagePath = "/uploads/" + uniqueFileName;
                }


                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "Mission edited successfully!";
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
            TempData["ToastMessage"] = "Mission deleted successfully!";
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
                TempData["ToastMessage"] = "Mission applied to successfully!";
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
