using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.BusinessObject.Models;
using FluentValidation;

namespace BulkyBook.BusinessObject.Validator
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotEqual(c=>c.DisplayOrder.ToString()).WithMessage("{PropertyName} and {ComparisonProperty} must not be the same");
            RuleFor(c => c.DisplayOrder)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .InclusiveBetween(0,100).WithMessage("{PropertyName} must be the range 0..100");
        }
    }
}
