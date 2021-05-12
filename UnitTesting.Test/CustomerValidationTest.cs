using FluentValidation.TestHelper;
using UnitTesting.API.Entities;
using UnitTesting.API.Validations;
using Xunit;

namespace UnitTesting.Test
{
    public class CustomerValidationTest
    {
        private readonly CustomerValidation _sut;

        public CustomerValidationTest()
        {
            _sut = new CustomerValidation();
        }

        [Fact]
        public void CustomerValidation_ShouldPassWhenNameIsNotNull()
        {
            _sut.ShouldNotHaveValidationErrorFor(
                s=>s.Name,
                new Customer()
                {
                    Name = "Test"
                }
                );
        }

        [Fact]
        public void CustomerValidation_ShouldFailWhenNameIsNull()
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
        public void CustomerValidation_ShouldPassWhenAgeGreaterThanOrEqualTo18()
        {
            _sut.ShouldNotHaveValidationErrorFor(
                s => s.Age,
                new Customer()
                {
                    Age = 18
                }
                );
        }

        [Fact]
        public void CustomerValidation_ShouldFailWhenAgeLessThan18()
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
        public void CustomerValidation_ShouldPassWhenValidEmailAddressFormate()
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
        public void CustomerValidation_ShouldFailWhenInvalidEmailAddressFormate()
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
