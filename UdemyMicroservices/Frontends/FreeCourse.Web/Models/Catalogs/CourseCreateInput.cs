using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalogs
{
    public class CourseCreateInput
    {
        [Display(Name="Adı")]
        public string Name { get; set; }

        [Display(Name="Açıklma")]
        public string Description { get; set; }

        [Display(Name = "Kurs Fiyatı")]
        public decimal Price { get; set; }

        public string Picture { get; set; }
        public string UserId { get; set; }
        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs Kategori")]
        public string CategoryId { get; set; }
        [Display(Name="Kurs Resmi")]
        public IFormFile PhotoFormFile{ get; set; }
    }
}
