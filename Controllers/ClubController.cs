using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RUnGroopWebApp.Data;
using RUnGroopWebApp.Models;

namespace RUnGroopWebApp.Controllers
{
    public class ClubController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ClubController(ApplicationDbContext context)
        {
            _context = context; //Injecting the DB here
        }


        public IActionResult Index() //// CCCCCCC
        {
            var clubs = _context.Clubs.ToList(); //// MMMMMMMMM
            return View(clubs); ////// VVVVVVVVV
        }

        public IActionResult Detail(int id) {

            Club club = _context.Clubs.Include(a => a.Address).FirstOrDefault(x => x.Id == id);
            return View(club);
        }
    }
}
