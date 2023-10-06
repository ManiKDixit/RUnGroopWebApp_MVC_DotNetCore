using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RUnGroopWebApp.Data;
using RUnGroopWebApp.Interfaces;
using RUnGroopWebApp.Models;
using RUnGroopWebApp.Repository;
using RUnGroopWebApp.ViewModels;

namespace RUnGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        //private readonly IDashboardRepository _dashboardRepository;
        //private readonly IPhotoService _photoService;

        //public DashboardController(IDashboardRepository dashboardRepository, IPhotoService photoService)
        //{
        //    _dashboardRepository = dashboardRepository;

        //    _photoService = photoService;

        //}

        //public async Task<IActionResult> Index()
        //{
        //    var userClubs = await _dashboardRepository.GetAllUserClubs();
        //    var userRaces = await _dashboardRepository.GetAllUserRaces();
        //    var dashboardViewModel = new DashboardViewModel()
        //    {
        //        Races = userRaces,
        //        Clubs = userClubs,
        //    };
        //    return View(dashboardViewModel);
        //}



        private readonly IDashboardRepository _dashboardRespository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpcontextAccessor;

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService, IHttpContextAccessor httpcontextAccessor)
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
            _httpcontextAccessor = httpcontextAccessor;
        }

        private void MapUserEdit(AppUser user , EditUserDashboardViewModel editVm , ImageUploadResult photoResult)
        {
            user.Id = editVm.Id;
            user.Pace = editVm.Pace;
            user.Mileage = editVm.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = editVm.City;
            user.State = editVm.State;
        }

        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRespository.GetAllUserRaces();
            var userClubs = await _dashboardRespository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpcontextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRespository.GetUserById(curUserId);
            if (user == null) return View("Error");
            var editUserViewModel = new EditUserDashboardViewModel()
            {  
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
                
            };
            return View(editUserViewModel);
        }

        

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            //var curUser = _httpcontextAccessor.HttpContext.User.GetUserId();
            //editVM.Id = curUser;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVM);
            }

            AppUser user = await _dashboardRespository.GetByIdNoTracking(editVM.Id);

            if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {

                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);

                _dashboardRespository.Update(user);

                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);

                _dashboardRespository.Update(user);

                return RedirectToAction("Index");
            }
        }

        //public async Task<IActionResult> EditUserProfile()
        //{
        //    var curUserId = _httpcontextAccessor.HttpContext.User.GetUserId();
        //    var user = await _dashboardRespository.GetUserById(curUserId);
        //    if (user == null)
        //    {
        //        return View("Error");
        //    }

        //    var editUserViewModel = new EditUserDashboardViewModel()
        //    {
        //        Id = curUserId,
        //        Pace = user.Pace,
        //        Mileage = user.Mileage,
        //        ProfileImageUrl = user.ProfileImageUrl,
        //        City = user.City,
        //        State = user.State,
        //    };

        //    return View(editUserViewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Failed to Edit Profile");
        //        return View("EditUserProfile", editVM);
        //    }

        //    AppUser user = await _dashboardRespository.GetByIdNoTracking(editVM.Id);

        //    if(user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
        //    {
        //        var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

        //        //Optimistic Concurrency - Tracking Error
        //       // Use No Tracking - Best way to deal with this error 
        //       // Otherise can create a seperare Map method as well , like we're doing here

        //        MapUserEdit(user , editVM, photoResult);

        //        _dashboardRespository.Update(user);

        //        return RedirectToAction("Index");

        //    }
        //    else
        //    {
        //        try
        //        {
        //            await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", "Could not delete photo");
        //            return View(editVM);
        //        }

        //        var photoResult = await _photoService.AddPhotoAsync(editVM.Image);


        //        MapUserEdit(user, editVM, photoResult);

        //        _dashboardRespository.Update(user);

        //        return RedirectToAction("Index");
        //    }
        //}
    }
}
