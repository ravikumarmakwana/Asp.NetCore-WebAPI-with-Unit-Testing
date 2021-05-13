using FluentValidation.TestHelper;
using UnitTesting.Entities;
using UnitTesting.Validations;
using Xunit;

namespace UnitTesting.Test
{
    public class CustomerValidatorTest
    {
        private readonly CustomerValidation _sut;

        public CustomerValidatorTest()
        {
            _sut = new CustomerValidation();
        }

        [Fact]
        public void CustomerValidator_ShouldPassWhenNameIsNotNull()
        {
            _sut.ShouldNotHaveValidationErrorFor(
                s => s.Name,
                new Customer()
                {
                    Name = "Test"
                }
                );
        }

        [Fact]
        public void CustomerValidator_ShouldFailWhenNameIsNull()
        {
            _sut.ShouldHaveValidationErrorFor(
                s => s.Name,
                new Customer()
                {
                    Name = null
                }
                ).WithErrorMessage(CustomerValidation.NameRequired);
        }

        [Fact]
        public void CustomerValidator_ShouldPassWhenAgeIsGreaterThanOrEqualTo18()
        {
            _sut.ShouldNotHaveValidationErrorFor(
                s => s.Age,
                new Customer()
                {
                    Age = 19
                }
                );
        }

        [Fact]
        public void CustomerValidator_ShouldFailWhenAgeIsLessThan18()
        {
            _sut.ShouldHaveValidationErrorFor(
                s => s.Age,
                new Customer()
                {
                    Age = 15
                }
                ).WithErrorMessage(CustomerValidation.AgeMustBeGreaterThanOrEqualTo18);
        }

        [Fact]
        public void CustomerValidator_ShouldPassWhenEmailAddressShouldHaveValidFormate()
        {
            _sut.ShouldNotHaveValidationErrorFor(
                s => s.EmailAddress,
                new Customer()
                {
                    EmailAddress = "ravi.makwan@gmail.com"
                }
                );
        }

        [Fact]
        public void CustomerValidatpr_ShouldFailWhenEmailAddressShouldNotHaveValidFormate()
        {
            _sut.ShouldHaveValidationErrorFor(
                s => s.EmailAddress,
                new Customer()
                {
                    EmailAddress = "ravi.com"
                }
                ).WithErrorMessage(CustomerValidation.InvalidEmailAddress);
        }
    }
}
