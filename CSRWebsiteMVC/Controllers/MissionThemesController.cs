using CSRWebsiteMVC.Data;
using CSRWebsiteMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSRWebsiteMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MissionThemesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MissionThemesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.MissionThemes.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MissionTheme theme)
        {
            if (ModelState.IsValid)
            {
                _context.Add(theme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(theme);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var theme = await _context.MissionThemes.FindAsync(id);
            if (theme == null) return NotFound();
            return View(theme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MissionTheme theme)
        {
            if (id != theme.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(theme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(theme);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var theme = await _context.MissionThemes.FindAsync(id);
            if (theme == null) return NotFound();
            return View(theme);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var theme = await _context.MissionThemes.FindAsync(id);
            if (theme != null)
            {
                _context.MissionThemes.Remove(theme);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
