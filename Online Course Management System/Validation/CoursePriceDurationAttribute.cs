using System.ComponentModel.DataAnnotations;

namespace Online_Course_Management_System.Validation
{
    public class CoursePriceDurationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance;

            var priceProperty =
                validationContext.ObjectType
                    .GetProperty("Price");

            var durationProperty =
                validationContext.ObjectType
                    .GetProperty("Duration");

            if (priceProperty == null ||
                durationProperty == null)
            {
                return ValidationResult.Success;
            }

            var price = (decimal)priceProperty.GetValue(model)!;
            var duration = (int)durationProperty.GetValue(model)!;

            if (price > 1000 && duration < 2)
            {
                return new ValidationResult(
                    "Courses costing more than 1000 must have at least 2 hours.");
            }

            return ValidationResult.Success;

        }
    }
}