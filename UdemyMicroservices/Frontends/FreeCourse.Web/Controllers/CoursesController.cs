﻿using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Catalogs;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;
        //private readonly 

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId));
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View();
            }

            courseCreateInput.UserId = _sharedIdentityService.GetUserId;
            await _catalogService.CreateCourseAsync(courseCreateInput);



            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(string Id)
        {
            var course = await _catalogService.GetByCourseIdAsync(Id);
            var categories = await _catalogService.GetAllCategoryAsync();
           
            if (course != null)
            {
                ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.CategoryId);
                CourseUpdateInput courseUpdateInput = new CourseUpdateInput
                {
                    CategoryId = course.CategoryId,
                    Description = course.Description,
                    Feature = course.Feature,
                    Name = course.Name,
                    Price = course.Price,
                    Picture = course.Picture,
                    Id = course.Id,
                    UserId=course.UserId
                };
                return View(courseUpdateInput);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _catalogService.UpdateCourseAsync(courseUpdateInput);
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Delete(string Id)
        {
            await _catalogService.DeleteCourseAsync(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
