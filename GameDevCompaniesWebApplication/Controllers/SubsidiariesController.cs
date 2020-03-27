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
    public class SubsidiariesController : Controller
    {
        private readonly GameDevCompaniesContext _context;

        public SubsidiariesController(GameDevCompaniesContext context)
        {
            _context = context;
        }

        // GET: Subsidiaries
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("GameDevCompanies", "Index");

            ViewBag.CompanyId = id;
            ViewBag.CompanyName = name;
            var gameDevCompaniesContext = _context.Subsidiaries.Where(s => s.CompanyId == id).Include(s => s.Company);
            
            return View(await gameDevCompaniesContext.ToListAsync());
        }

        // GET: Subsidiaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subsidiaries = await _context.Subsidiaries
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subsidiaries == null)
            {
                return NotFound();
            }

            return View(subsidiaries);
        }

        public async Task<IActionResult> Games(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subsidiaries = await _context.Subsidiaries
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subsidiaries == null)
            {
                return NotFound();
            }

            //return View(subsidiaries);
            return RedirectToAction("Index", "GamesDevelopers", new { id = subsidiaries.Id, name = subsidiaries.Name });
        }

        // GET: Subsidiaries/Create
        public IActionResult Create(int CompanyId)
        {
            //ViewData["CompanyId"] = new SelectList(_context.GameDevCompanies, "Id", "DirectorFullName");
            ViewBag.CompanyId = CompanyId;
            ViewBag.CompanyName = _context.GameDevCompanies.Where(c => c.Id == CompanyId).FirstOrDefault().Name;
            return View();
        }

        // POST: Subsidiaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int CompanyId, [Bind("Id,Name,Location,CompanyId,ManagerFullName")] Subsidiaries subsidiaries)
        {
            subsidiaries.CompanyId = CompanyId;

            if (ModelState.IsValid)
            {
                _context.Add(subsidiaries);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Subsidiaries", new { id = CompanyId, name = _context.GameDevCompanies.Where(c => c.Id == CompanyId).FirstOrDefault().Name });
            }
            //ViewData["CompanyId"] = new SelectList(_context.GameDevCompanies, "Id", "DirectorFullName", subsidiaries.CompanyId);
            //return View(subsidiaries);
            return RedirectToAction("Index", "Subsidiaries", new { id = CompanyId, name = _context.GameDevCompanies.Where(c => c.Id == CompanyId).FirstOrDefault().Name });
        }

        // GET: Subsidiaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subsidiaries = await _context.Subsidiaries.FindAsync(id);
            if (subsidiaries == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.GameDevCompanies, "Id", "Name", subsidiaries.CompanyId);
            return View(subsidiaries);
        }

        // POST: Subsidiaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,CompanyId,ManagerFullName")] Subsidiaries subsidiaries)
        {
            if (id != subsidiaries.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subsidiaries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubsidiariesExists(subsidiaries.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.GameDevCompanies, "Id", "DirectorFullName", subsidiaries.CompanyId);
            return View(subsidiaries);
        }

        // GET: Subsidiaries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subsidiaries = await _context.Subsidiaries
                .Include(s => s.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subsidiaries == null)
            {
                return NotFound();
            }

            return View(subsidiaries);
        }

        // POST: Subsidiaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subsidiaries = await _context.Subsidiaries.FindAsync(id);
            _context.Subsidiaries.Remove(subsidiaries);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "GameDevCompanies");
        }

        private bool SubsidiariesExists(int id)
        {
            return _context.Subsidiaries.Any(e => e.Id == id);
        }
    }
}
