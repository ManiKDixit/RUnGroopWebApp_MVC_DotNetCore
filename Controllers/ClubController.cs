using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RUnGroopWebApp.Data;
using RUnGroopWebApp.Interfaces;
using RUnGroopWebApp.Models;

namespace RUnGroopWebApp.Controllers
{
    public class ClubController : Controller
    {

       // private readonly ApplicationDbContext _context;
        private readonly IClubRepository _clubRepository;

        public ClubController(ApplicationDbContext context , IClubRepository clubRepository)
        {
           // _context = context; //Injecting the DB here
            _clubRepository = clubRepository;
        }


        public async Task<IActionResult> Index() //// CCCCCCC
        {
            var clubs = await _clubRepository.GetAll(); //// MMMMMMMMM
            return View(clubs); ////// VVVVVVVVV
        }

        public async Task<IActionResult> Detail(int id) {

            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create ()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (Club club)
        {
            if(!ModelState.IsValid)
            {
                return View(club);
            }
            _clubRepository.Add(club);
            return RedirectToAction("Index");

        }
    }
}
