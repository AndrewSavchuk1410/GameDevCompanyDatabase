using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GameDevCompaniesWebApplication;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream,
            XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                GameDevCompanies newCompanies;
                                var c = (from company in _context.GameDevCompanies where company.Name.Contains(worksheet.Name) select company).ToList();
                                if (c.Count > 0)
                                {
                                    newCompanies = c[0];
                                }
                                else
                                {
                                    newCompanies = new GameDevCompanies();
                                    newCompanies.Name = worksheet.Name;
                                    newCompanies.Location = "From EXCEL";
                                    newCompanies.DirectorFullName = "From EXCEL";
                                    _context.GameDevCompanies.Add(newCompanies);
                                }
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {

                                    try
                                    {    
                                        Subsidiaries sub = new Subsidiaries();
                                        sub.Name = row.Cell(1).Value.ToString();
                                        sub.Location = row.Cell(2).Value.ToString();
                                        sub.ManagerFullName = row.Cell(3).Value.ToString();
                                        sub.Company = newCompanies;

                                        var arr = _context.Subsidiaries.Where(s => s.Name == sub.Name).ToList();
                                        if (arr.Count() == 0) _context.Subsidiaries.Add(sub);
                                    }
                                    catch (Exception e)
                                    {
                                        
                                    }
                                }
                            }

                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        public ActionResult Export(int id)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                
                var subsidiaries = _context.Subsidiaries.Where(s => s.CompanyId == id).ToList();
                var com = _context.GameDevCompanies.Where(c => c.Id == id).ToList();
                var worksheet = workbook.Worksheets.Add(com.FirstOrDefault().Name);
                worksheet.Cell("A1").Value = "Name" ;
                worksheet.Cell("B1").Value = "Location";
                worksheet.Cell("C1").Value = "Studio manager";
                worksheet.Row(1).Style.Font.Bold = true;

                    for (int i = 0; i < subsidiaries.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = subsidiaries[i].Name;
                        worksheet.Cell(i + 2, 2).Value = subsidiaries[i].Location;
                        worksheet.Cell(i + 2, 3).Value = subsidiaries[i].ManagerFullName;
                    }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                    FileDownloadName = $"library_{ DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }


        public ActionResult ReportWord()
        {
            using (var stream = new MemoryStream())
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    Paragraph para = body.AppendChild(new Paragraph());
                    para.ParagraphProperties = new ParagraphProperties(new Tabs(new TabStop() { Val = TabStopValues.Right, Position = 8000, Leader = TabStopLeaderCharValues.Dot }));
                    //Run run = para.AppendChild(new Run());
                    para.Append(new Run(new Text("Number of game development companies")));
                    para.Append(new Run(new TabChar()));
                    para.Append(new Run(new Text($"{_context.GameDevCompanies.Count()}")));
                    para.Append(new Run(new Break()));
                    para.Append(new Run(new Break()));


                    foreach (var c in _context.GameDevCompanies)
                    {
                        //para.Append(new Run(new TabChar()));
                        para.Append(new Run(new Text($"Number of subsidiaries of company {c.Name}")));
                        para.Append(new Run(new TabChar()));
                        para.Append(new Run(new Text($"{_context.Subsidiaries.Where(s => s.CompanyId == c.Id).Count()}")));
                        para.Append(new Run(new Break()));
                    }

                    para.Append(new Run(new Break()));
                    para.Append(new Run(new Break()));

                    para.Append(new Run(new Text("Number of computer games")));
                    para.Append(new Run(new TabChar()));
                    para.Append(new Run(new Text($"{_context.ComputerGames.Count()}")));
                    para.Append(new Run(new Break()));
                    para.Append(new Run(new Break()));


                    foreach (var g in _context.ComputerGames)
                    {
                        //para.Append(new Run(new TabChar()));
                        para.Append(new Run(new Text($"Number of genres of {g.Name}")));
                        para.Append(new Run(new TabChar()));
                        para.Append(new Run(new Text($"{_context.GamesGenres.Where(gen => gen.GameId == g.Id).Count()}")));
                        para.Append(new Run(new Break()));
                        para.Append(new Run(new Text($"Number of platforms of {g.Name}")));
                        para.Append(new Run(new TabChar()));
                        para.Append(new Run(new Text($"{_context.GamesPlatforms.Where(p => p.GameId == g.Id).Count()}")));
                        para.Append(new Run(new Break()));
                        para.Append(new Run(new Break()));
                    }

                    
                    para.Append(new Run(new Break()));

                    para.Append(new Run(new Text("Number of genres")));
                    para.Append(new Run(new TabChar()));
                    para.Append(new Run(new Text($"{_context.Genres.Count()}")));
                    para.Append(new Run(new Break()));

                    para.Append(new Run(new Break()));
                    para.Append(new Run(new Break()));

                    para.Append(new Run(new Text("Number of platforms")));
                    para.Append(new Run(new TabChar()));
                    para.Append(new Run(new Text($"{_context.Platforms.Count()}")));
                    para.Append(new Run(new Break()));

                    para.Append(new Run(new Break()));
                    para.Append(new Run(new Break()));

                    para.Append(new Run(new Text("Number of distributors")));
                    para.Append(new Run(new TabChar()));
                    para.Append(new Run(new Text($"{_context.Distributors.Count()}")));
                    para.Append(new Run(new Break()));

                    mainPart.Document.Save();
                    wordDocument.Close();
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                    {
                        FileDownloadName = $"report_{DateTime.UtcNow.ToShortDateString()}.docx"
                    };
                }
            }
        }
    }
}
