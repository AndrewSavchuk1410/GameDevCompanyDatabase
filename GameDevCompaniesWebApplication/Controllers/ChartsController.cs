using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameDevCompaniesWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly GameDevCompaniesContext _context;

        public ChartsController (GameDevCompaniesContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var gameDevCompanies = _context.GameDevCompanies.Include(c => c.Subsidiaries).ToList();

            List<object> comGame = new List<object>();

            comGame.Add(new[] { "Company", "Number of subsidiaries" });

            foreach (var c in gameDevCompanies)
            {
                comGame.Add(new object[] { c.Name, c.Subsidiaries.Count() });
            }

            return new JsonResult(comGame);
        }

        [HttpGet("JsonData2")]
        public JsonResult JsonData2()
        {
            //var computerGames = _context.ComputerGames.Include(g => g.Budget).ToList();

            List<object> gameBudget = new List<object>();
            
            gameBudget.Add(new[] { "Game", "Budget" });

            foreach (var g in _context.ComputerGames)
            {
                gameBudget.Add(new object[] { g.Name, g.Budget / 1000000 });
            }

            return new JsonResult(gameBudget);
        }

        [HttpGet("JsonData3")]
        public JsonResult JsonData3()
        {
            var computerGames = _context.ComputerGames;

            List<object> gamePlat = new List<object>();

            gamePlat.Add(new[] { "Game", "Platforms" });

            foreach (var g in computerGames)
            {
                var abc = _context.GamesPlatforms.Where(c => c.GameId == g.Id).Include(g => g.Platform).ToList();
                gamePlat.Add(new object[] { g.Name, abc.Count() });
            }

            return new JsonResult(gamePlat);
        }

    }
}