using AcmeWeb.Dal;
using FluentValidation;

namespace AcmeWeb
{
    class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(t => t.FirstName).NotEmpty();
            RuleFor(t => t.LastName).NotEmpty();
        }
    }
}