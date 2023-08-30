using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RUnGroopWebApp.Data;
using RUnGroopWebApp.Models;

namespace RUnGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RaceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
           List<Race> races = _context.Races.ToList();
            return View(races);
        }

        public IActionResult Detail(int id)
        {

            Race race = _context.Races.Include(a => a.Address).FirstOrDefault(x => x.Id == id);
            return View(race);
        }
    }
}
