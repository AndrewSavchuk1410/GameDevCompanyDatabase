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
    public class GamesGenresController : Controller
    {
        private readonly GameDevCompaniesContext _context;

        public GamesGenresController(GameDevCompaniesContext context)
        {
            _context = context;
        }

        // GET: GamesGenres
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("ComputerGames", "Index");
            ViewBag.GameId = id;
            ViewBag.GameName = name;
            var gameDevCompaniesContext = _context.GamesGenres.Where(c => c.GameId == id).Include(g => g.Game).Include(g => g.Genre);
            return View(await gameDevCompaniesContext.ToListAsync());
        }

        // GET: GamesGenres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesGenres = await _context.GamesGenres
                .Include(g => g.Game)
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gamesGenres == null)
            {
                return NotFound();
            }

            return View(gamesGenres);
        }

        // GET: GamesGenres/Create
        public IActionResult Create(int GameId)
        {
            //ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Engine");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.GameId = GameId;
            ViewBag.GameName = _context.ComputerGames.Where(c => c.Id == GameId).FirstOrDefault().Name;
            return View();
        }

        // POST: GamesGenres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int GameId, [Bind("Id,GenreId")] GamesGenres gamesGenres)
        {
            gamesGenres.GameId = GameId;
            if (ModelState.IsValid)
            {
                _context.Add(gamesGenres);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "GamesGenres", new { id = GameId, name = _context.ComputerGames.Where(c => c.Id == GameId).FirstOrDefault().Name });
            }
            //ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Name", gamesGenres.GameId);
            //ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", gamesGenres.GenreId);
            //return View(gamesGenres);
            return RedirectToAction("Index", "GamesGenres", new { id = GameId, name = _context.ComputerGames.Where(c => c.Id == GameId).FirstOrDefault().Name });
        }

        // GET: GamesGenres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesGenres = await _context.GamesGenres.FindAsync(id);
            if (gamesGenres == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Name", gamesGenres.GameId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", gamesGenres.GenreId);
            return View(gamesGenres);
        }

        // POST: GamesGenres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GameId,GenreId")] GamesGenres gamesGenres)
        {
            if (id != gamesGenres.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gamesGenres);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GamesGenresExists(gamesGenres.Id))
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
            ViewData["GameId"] = new SelectList(_context.ComputerGames, "Id", "Engine", gamesGenres.GameId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Info", gamesGenres.GenreId);
            return View(gamesGenres);
        }

        // GET: GamesGenres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gamesGenres = await _context.GamesGenres
                .Include(g => g.Game)
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gamesGenres == null)
            {
                return NotFound();
            }

            return View(gamesGenres);
        }

        // POST: GamesGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gamesGenres = await _context.GamesGenres.FindAsync(id);
            _context.GamesGenres.Remove(gamesGenres);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GamesGenresExists(int id)
        {
            return _context.GamesGenres.Any(e => e.Id == id);
        }
    }
}
