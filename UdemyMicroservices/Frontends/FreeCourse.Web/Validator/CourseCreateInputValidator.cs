using FluentValidation;
using FreeCourse.Web.Models.Catalogs;

namespace FreeCourse.Web.Validator
{
    public class CourseCreateInputValidator : AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Ad alanı boş olamaz");
            RuleFor(d => d.Description).NotEmpty().WithMessage("Açıklma alanı boş olamaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("süre alanı boş olamaz");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz").ScalePrecision(2, 6).WithMessage("Hatalı format");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori Seçilmeli");
        }
    }
}
