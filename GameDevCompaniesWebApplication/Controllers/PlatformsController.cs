using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameDevCompaniesWebApplication;

namespace GameDevCompaniesWebApplication.Controllers
{
    public class PlatformsController : Controller
    {
        private readonly GameDevCompaniesContext _context;

        public PlatformsController(GameDevCompaniesContext context)
        {
            _context = context;
        }

        // GET: Platforms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Platforms.ToListAsync());
        }

        // GET: Platforms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platforms = await _context.Platforms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platforms == null)
            {
                return NotFound();
            }

            return View(platforms);
        }

        // GET: Platforms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Platforms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Info")] Platforms platforms)
        {
            if (ModelState.IsValid)
            {
                _context.Add(platforms);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(platforms);
        }

        // GET: Platforms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platforms = await _context.Platforms.FindAsync(id);
            if (platforms == null)
            {
                return NotFound();
            }
            return View(platforms);
        }

        // POST: Platforms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Info")] Platforms platforms)
        {
            if (id != platforms.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(platforms);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatformsExists(platforms.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(platforms);
        }

        // GET: Platforms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platforms = await _context.Platforms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platforms == null)
            {
                return NotFound();
            }

            return View(platforms);
        }

        // POST: Platforms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var platforms = await _context.Platforms.FindAsync(id);
            _context.Platforms.Remove(platforms);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatformsExists(int id)
        {
            return _context.Platforms.Any(e => e.Id == id);
        }
    }
}
