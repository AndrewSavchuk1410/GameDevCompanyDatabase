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
    public class GamesPlatformsController : Controller
    {
        private readonly GameDevCompaniesContext _context;

        public GamesPlatformsController(GameDevCompaniesContext context)
        {
            _context = context;
        }

        // GET: GamesPlatforms
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("CoputerGames", "Index");
            ViewBag.GameId = id;
            ViewBag.GameName = name;
            var gameDevCompaniesContext = _context.GamesPlatforms.Where(c => c.GameId == id).Include(g => g.Game).Include(g => g.Platform);
            return View(await gameDevCompaniesContext.ToListAsync());
        }

        // GET: GamesPlatforms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesPlatforms = await _context.GamesPlatforms
                .Include(g => g.Game)
                .Include(g => g.Platform)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gamesPlatforms == null)
            {
                return NotFound();
            }

            return View(gamesPlatforms);
        }

        // GET: GamesPlatforms/Create
        public IActionResult Create(int GameId)
        {
            //ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Engine");
            ViewData["PlatformId"] = new SelectList(_context.Platforms, "Id", "Name");
            ViewBag.GameId = GameId;
            ViewBag.GameName = _context.ComputerGames.Where(c => c.Id == GameId).FirstOrDefault().Name;
            return View();
        }

        // POST: GamesPlatforms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int GameId, [Bind("Id,PlatformId")] GamesPlatforms gamesPlatforms)
        {
            gamesPlatforms.GameId = GameId;
            if (ModelState.IsValid)
            {
                _context.Add(gamesPlatforms);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "GamesPlatforms", new { id = GameId, name = _context.ComputerGames.Where(c => c.Id == GameId).FirstOrDefault().Name });
            }
            //ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Engine", gamesPlatforms.GameId);
            //ViewData["PlatformId"] = new SelectList(_context.Platforms, "Id", "Info", gamesPlatforms.PlatformId);
            //return View(gamesPlatforms);
            return RedirectToAction("Index", "GamesPlatforms", new { id = GameId, name = _context.ComputerGames.Where(c => c.Id == GameId).FirstOrDefault().Name });
        }

        // GET: GamesPlatforms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesPlatforms = await _context.GamesPlatforms.FindAsync(id);
            if (gamesPlatforms == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Name", gamesPlatforms.GameId);
            ViewData["PlatformId"] = new SelectList(_context.Platforms, "Id", "Name", gamesPlatforms.PlatformId);
            return View(gamesPlatforms);
        }

        // POST: GamesPlatforms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GameId,PlatformId")] GamesPlatforms gamesPlatforms)
        {
            if (id != gamesPlatforms.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gamesPlatforms);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GamesPlatformsExists(gamesPlatforms.Id))
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
            ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Name", gamesPlatforms.GameId);
            ViewData["PlatformId"] = new SelectList(_context.Platforms, "Id", "Name", gamesPlatforms.PlatformId);
            return View(gamesPlatforms);
        }

        // GET: GamesPlatforms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesPlatforms = await _context.GamesPlatforms
                .Include(g => g.Game)
                .Include(g => g.Platform)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gamesPlatforms == null)
            {
                return NotFound();
            }

            return View(gamesPlatforms);
        }

        // POST: GamesPlatforms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gamesPlatforms = await _context.GamesPlatforms.FindAsync(id);
            _context.GamesPlatforms.Remove(gamesPlatforms);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ComputerGames");
        }

        private bool GamesPlatformsExists(int id)
        {
            return _context.GamesPlatforms.Any(e => e.Id == id);
        }
    }
}
