using Microsoft.AspNetCore.Mvc;
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
    }
}
