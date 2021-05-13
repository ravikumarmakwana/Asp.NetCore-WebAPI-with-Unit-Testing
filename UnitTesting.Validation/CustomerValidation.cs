using FluentValidation;
using UnitTesting.Entities;

namespace UnitTesting.Validations
{
    public class CustomerValidation : AbstractValidator<Customer>
    {
        public const string NameRequired = "Customer name must be not null";
        public const string AgeMustBeGreaterThanOrEqualTo18 = "Customer Age must be greater than or equal to 18";
        public const string InvalidEmailAddress = "Please enter the valid email";

        [System.Obsolete]
        public CustomerValidation()
        {
            RuleFor(customer => customer.Name)
                .NotNull()
                .WithMessage(NameRequired);
            
            RuleFor(customer => customer.Age)
                .GreaterThanOrEqualTo(18)
                .WithMessage(AgeMustBeGreaterThanOrEqualTo18);
            
            RuleFor(customer => customer.EmailAddress)
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.Net4xRegex)
                .WithMessage(InvalidEmailAddress);
        }
    }
}
