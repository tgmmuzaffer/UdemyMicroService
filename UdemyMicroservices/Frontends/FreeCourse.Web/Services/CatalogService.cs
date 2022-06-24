﻿using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Catalogs;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IPhotoStockService _photoStockService;
        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService = null)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var resultPhotoSerive = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);
            if(!resultPhotoSerive.Equals(null))
            {
                courseCreateInput.Picture = resultPhotoSerive.Url;
            }
            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("courses", courseCreateInput);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _httpClient.DeleteAsync($"courses/{courseId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _httpClient.GetAsync("categories");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responsesuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
            return responsesuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            var response = await _httpClient.GetAsync("courses");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responsesuccess = await response.Content.ReadFromJsonAsync <Response<List<CourseViewModel>>>();
            return responsesuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"courses/GetAllByUserId/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responsesuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            return responsesuccess.Data;
        }

        public async Task<CourseViewModel> GetByCourseIdAsync(string courseId)
        {
            var response = await _httpClient.GetAsync($"courses/{courseId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responsesuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            return responsesuccess.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("courses", courseUpdateInput);
            return response.IsSuccessStatusCode;
        }
    }
}
