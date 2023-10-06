using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RUnGroopWebApp.Data;
using RUnGroopWebApp.Data.Enum;
using RUnGroopWebApp.Interfaces;
using RUnGroopWebApp.Models;
using RUnGroopWebApp.ViewModels;

namespace RUnGroopWebApp.Controllers
{
    public class ClubController : Controller
    {

       // private readonly ApplicationDbContext _context;
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(ApplicationDbContext context , IClubRepository clubRepository , IPhotoService photoService , IHttpContextAccessor httpContextAccessor)
        {
           // _context = context; //Injecting the DB here
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;  
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


        //[HttpGet]
        //public IActionResult Create()
        //{
        //    var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
        //    var createClubViewModel = new CreateClubViewModel { AppUserId = curUserID };
        //    return View(createClubViewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create (CreateClubViewModel clubVM)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        // return View(club);
        //        var result = await _photoService.AddPhotoAsync(clubVM.Image);
        //        var club = new Club
        //        {
        //            Title = clubVM.Title,
        //            Description = clubVM.Description,
        //            Image = result.Url.ToString(),
        //            AppUserId = clubVM.AppUserId,
        //            Address = new Address
        //            {
        //                Street = clubVM.Address.Street,
        //                City = clubVM.Address.City,
        //                State = clubVM.Address.State,
        //            }
        //        };

        //        _clubRepository.Add(club);
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Photo upload failed");
        //    }

        //    return View(clubVM);

        //}



        [HttpGet]
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel { AppUserId = curUserId };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    ClubCategory = clubVM.ClubCategory,
                    AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVM);
        }

        public async Task<IActionResult> Edit (int id) {

            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var club = await _clubRepository.GetByIdAsync (id);
            if(club == null)
            {
                return View("Error");
            }

            var clubVM = new EditClubViewModel
            {
                //AppUserId = curUserId,
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image.ToString(),
                ClubCategory = club.ClubCategory

            };

            return  View(clubVM);

        }



        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

            if (userClub == null)
            {
                return View("Error");
            }

            var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(clubVM);
            }

            if (!string.IsNullOrEmpty(userClub.Image))
            {
                _ = _photoService.DeletePhotoAsync(userClub.Image);
            }

            var club = new Club
            {
                Id = id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                //AppUserId =  clubVM.AppUserId,
                Image = photoResult.Url.ToString(),
                AddressId = clubVM.AddressId,
                Address = clubVM.Address,
            };

            _clubRepository.Update(club);

            return RedirectToAction("Index");
        }
    }
}


/*
 * // This would actually pass the user Id in the Getuser method as a paramteer without explicitly writing the parameter , so here we're basicallhy fetching the user id , without going to the Database.
 */