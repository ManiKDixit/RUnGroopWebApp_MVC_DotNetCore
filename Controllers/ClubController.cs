using Microsoft.AspNetCore.Mvc;
using RUnGroopWebApp.Data;

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
    }
}
