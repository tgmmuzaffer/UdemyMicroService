using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalogs
{
    public class CourseCreateInput
    {
        [Display(Name="Adı")]
        [Required]
        public string Name { get; set; }

        [Display(Name="Açıklma")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Kurs Fiyatı")]
        [Required]
        public decimal Price { get; set; }

        public string Picture { get; set; }
        public string UserId { get; set; }
        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs Kategori")]
        [Required]
        public string CategoryId { get; set; }
        [Display(Name="Kurs Resmi")]
        public IFormFile PhotoFormFile{ get; set; }
    }
}
