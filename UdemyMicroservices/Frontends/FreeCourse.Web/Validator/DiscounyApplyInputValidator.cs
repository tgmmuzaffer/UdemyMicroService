using FluentValidation;
using FreeCourse.Web.Models.Discounts;

namespace FreeCourse.Web.Validator
{
    public class DiscounyApplyInputValidator : AbstractValidator<DiscountApplyInput>
    {
        public DiscounyApplyInputValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Indirim kupon alanı boş olamaz");
        }
    }
}