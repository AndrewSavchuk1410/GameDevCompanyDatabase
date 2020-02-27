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
    public class ComputerGamesController : Controller
    {
        private readonly GameDevCompaniesContext _context;

        public ComputerGamesController(GameDevCompaniesContext context)
        {
            _context = context;
        }

        // GET: ComputerGames
        public async Task<IActionResult> Index()
        {
            return View(await _context.ComputerGames.ToListAsync());
        }

        // GET: ComputerGames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computerGames = await _context.ComputerGames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computerGames == null)
            {
                return NotFound();
            }

            return View(computerGames);
        }

        // GET: ComputerGames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ComputerGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Budget,Engine")] ComputerGames computerGames)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computerGames);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(computerGames);
        }

        // GET: ComputerGames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computerGames = await _context.ComputerGames.FindAsync(id);
            if (computerGames == null)
            {
                return NotFound();
            }
            return View(computerGames);
        }

        // POST: ComputerGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Budget,Engine")] ComputerGames computerGames)
        {
            if (id != computerGames.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computerGames);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputerGamesExists(computerGames.Id))
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
            return View(computerGames);
        }

        // GET: ComputerGames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computerGames = await _context.ComputerGames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computerGames == null)
            {
                return NotFound();
            }

            return View(computerGames);
        }

        // POST: ComputerGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var computerGames = await _context.ComputerGames.FindAsync(id);
            _context.ComputerGames.Remove(computerGames);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComputerGamesExists(int id)
        {
            return _context.ComputerGames.Any(e => e.Id == id);
        }
    }
}
