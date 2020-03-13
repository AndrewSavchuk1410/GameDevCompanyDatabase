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
    public class GameDevCompaniesController : Controller
    {
        private readonly GameDevCompaniesContext _context;

        public GameDevCompaniesController(GameDevCompaniesContext context)
        {
            _context = context;
        }

        // GET: GameDevCompanies
        public async Task<IActionResult> Index()
        {
            return View(await _context.GameDevCompanies.ToListAsync());
        }

        // GET: GameDevCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDevCompanies = await _context.GameDevCompanies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameDevCompanies == null)
            {
                return NotFound();
            }

            //return View(gameDevCompanies);
            return RedirectToAction("Index", "Subsidiaries", new { id = gameDevCompanies.Id, name = gameDevCompanies.Name });
        }

        // GET: GameDevCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameDevCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,DirectorFullName")] GameDevCompanies gameDevCompanies)
        {
            GameDevCompanies existingCompany = await _context.GameDevCompanies.FirstOrDefaultAsync(
                c => c.Name == gameDevCompanies.Name && c.Location == gameDevCompanies.Location); 

            if (existingCompany != null)
            {
                ModelState.AddModelError(string.Empty, "This company already exists.");
            }

            else if (ModelState.IsValid)
            {
                _context.Add(gameDevCompanies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameDevCompanies);
        }

        // GET: GameDevCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDevCompanies = await _context.GameDevCompanies.FindAsync(id);
            if (gameDevCompanies == null)
            {
                return NotFound();
            }
            return View(gameDevCompanies);
        }

        // POST: GameDevCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,DirectorFullName")] GameDevCompanies gameDevCompanies)
        {
            if (id != gameDevCompanies.Id)
            {
                return NotFound();
            }

            GameDevCompanies existingCompany = await _context.GameDevCompanies.FirstOrDefaultAsync(
                c => c.Name == gameDevCompanies.Name && c.Location == gameDevCompanies.Location);

            if (existingCompany != null)
            {
                ModelState.AddModelError(string.Empty, "This company already exists.");
            }

            else if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameDevCompanies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameDevCompaniesExists(gameDevCompanies.Id))
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
            return View(gameDevCompanies);
        }

        // GET: GameDevCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDevCompanies = await _context.GameDevCompanies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameDevCompanies == null)
            {
                return NotFound();
            }

            return View(gameDevCompanies);
        }

        // POST: GameDevCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameDevCompanies = await _context.GameDevCompanies.FindAsync(id);
            _context.GameDevCompanies.Remove(gameDevCompanies);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameDevCompaniesExists(int id)
        {
            return _context.GameDevCompanies.Any(e => e.Id == id);
        }
    }
}
