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
    public class GamesDevelopersController : Controller
    {
        private readonly GameDevCompaniesContext _context;

        public GamesDevelopersController(GameDevCompaniesContext context)
        {
            _context = context;
        }

        // GET: GamesDevelopers
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Subsidiaries", "Index");
            ViewBag.SubsidiariesId = id;
            ViewBag.SubsidiariesName = name;
            var gameDevCompaniesContext = _context.GamesDevelopers.Where(s => s.SubsidiariesId == id).Include(g => g.Subsidiaries).Include(g => g.Game);
            return View(await gameDevCompaniesContext.ToListAsync());
        }

        // GET: GamesDevelopers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesDevelopers = await _context.GamesDevelopers
                .Include(g => g.Game)
                .Include(g => g.Subsidiaries)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gamesDevelopers == null)
            {
                return NotFound();
            }

            return View(gamesDevelopers);
        }

        // GET: GamesDevelopers/Create
        public IActionResult Create(int SubsidiariesId)
        {
            ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Name");
            //ViewData["SubsidiariesId"] = new SelectList(_context.Subsidiaries, "Id", "Location");
            ViewBag.SubsidiariesId = SubsidiariesId;
            ViewBag.SubsidiariesName = _context.Subsidiaries.Where(s => s.Id == SubsidiariesId).FirstOrDefault().Name;
            return View();
        }

        // POST: GamesDevelopers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int SubsidiariesId, [Bind("Id,GameId")] GamesDevelopers gamesDevelopers)
        {
            gamesDevelopers.SubsidiariesId = SubsidiariesId;
            if (ModelState.IsValid)
            {
                _context.Add(gamesDevelopers);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "GamesDevelopers", new { id = SubsidiariesId, name = _context.Subsidiaries.Where(s => s.Id == SubsidiariesId).FirstOrDefault().Name });
            }
            //ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Engine", gamesDevelopers.GameId);
            //ViewData["SubsidiariesId"] = new SelectList(_context.Subsidiaries, "Id", "Location", gamesDevelopers.SubsidiariesId);
            return RedirectToAction("Index", "GamesDevelopers", new { id = SubsidiariesId, name = _context.Subsidiaries.Where(s => s.Id == SubsidiariesId).FirstOrDefault().Name });
        }

        // GET: GamesDevelopers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesDevelopers = await _context.GamesDevelopers.FindAsync(id);
            if (gamesDevelopers == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Name", gamesDevelopers.GameId);
            ViewData["SubsidiariesId"] = new SelectList(_context.Subsidiaries, "Id", "Name", gamesDevelopers.SubsidiariesId);
            return View(gamesDevelopers);
        }

        // POST: GamesDevelopers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubsidiariesId,GameId")] GamesDevelopers gamesDevelopers)
        {
            if (id != gamesDevelopers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gamesDevelopers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GamesDevelopersExists(gamesDevelopers.Id))
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
            ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Engine", gamesDevelopers.GameId);
            ViewData["SubsidiariesId"] = new SelectList(_context.Subsidiaries, "Id", "Location", gamesDevelopers.SubsidiariesId);
            return View(gamesDevelopers);
        }

        // GET: GamesDevelopers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesDevelopers = await _context.GamesDevelopers
                .Include(g => g.Game)
                .Include(g => g.Subsidiaries)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gamesDevelopers == null)
            {
                return NotFound();
            }

            return View(gamesDevelopers);
        }

        // POST: GamesDevelopers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gamesDevelopers = await _context.GamesDevelopers.FindAsync(id);
            _context.GamesDevelopers.Remove(gamesDevelopers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GamesDevelopersExists(int id)
        {
            return _context.GamesDevelopers.Any(e => e.Id == id);
        }
    }
}
