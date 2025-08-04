using CSRWebsiteMVC.Data;
using CSRWebsiteMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSRWebsiteMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MissionSkillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MissionSkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.MissionSkills.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MissionSkill missionSkill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(missionSkill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(missionSkill);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var missionSkill = await _context.MissionSkills.FindAsync(id);
            if (missionSkill == null) return NotFound();

            return View(missionSkill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MissionSkill missionSkill)
        {
            if (id != missionSkill.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(missionSkill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(missionSkill);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var missionSkill = await _context.MissionSkills.FindAsync(id);
            if (missionSkill == null) return NotFound();

            return View(missionSkill);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var missionSkill = await _context.MissionSkills.FindAsync(id);
            if (missionSkill != null)
            {
                _context.MissionSkills.Remove(missionSkill);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
