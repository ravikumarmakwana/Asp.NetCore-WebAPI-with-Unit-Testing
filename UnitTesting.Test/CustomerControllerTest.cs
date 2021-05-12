using FluentAssertions;
using Moq;
using UnitTesting.API.Controller;
using UnitTesting.API.Entities;
using UnitTesting.API.Services.interfaces;
using UnitTesting.API.Validations;
using Xunit;

namespace UnitTesting.Test
{
    public class CustomerControllerTest
    {
        private readonly CustomerController _sut;
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomerValidation _customerValidation;
        private readonly Mock<ILogger> _loggerMock;

        public CustomerControllerTest()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _customerValidation = new CustomerValidation();
            _loggerMock = new Mock<ILogger>();

            _sut = new CustomerController(_loggerMock.Object, _customerValidation, _customerServiceMock.Object);
        }

        [Fact]
        public void CustomerController_ShouldPassWhen_CustomerAddedSuccessfully()
        {
            //Arrange
            var customerToAdd = new Customer()
            {
                Age = 18,
                Name = "Test",
                EmailAddress = "Test@google.com"
            };

            var addedCustomer = new Customer()
            {
                Id = 1,
                Age = 18,
                Name = "Test",
                EmailAddress = "Test@google.com"
            };

            _customerServiceMock.Setup(s => s.Add(customerToAdd)).Returns(addedCustomer);

            //Act
            var validationResult = _customerValidation.Validate(customerToAdd);
            var result = _sut.Add(customerToAdd);

            //Assert
            validationResult.IsValid.Should().BeTrue();
            _loggerMock.Verify(s => s.LogInformation("Customer Added Successfully"), Times.Once);
            result.Should().Be(addedCustomer);
        }

        [Fact]
        public void CustomerController_ShouldFailWhen_CustomerNotAddedSuccessfully()
        {
            //Arrange
            var customerToAdd = new Customer()
            {
                Age = 10,
                Name = "Test",
                EmailAddress = "Test@tarktech.com"
            };

            //Act
            var validationResult = _customerValidation.Validate(customerToAdd);
            var result = _sut.Add(customerToAdd);

            //Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.ToString().Should().Be(CustomerValidation.AgeMustBeGreaterThanOrEqualTo18);
            _loggerMock.Verify(s => s.LogInformation("Customer Added Successfully"), Times.Never);
            result.Should().Be(null);
        }

        [Fact]
        public void CustomerController_ShouldPassWhen_ReturnExistingCustomer()
        {
            //Arrange
            var customer = new Customer()
            {
                Id = 1,
                Age = 100,
                Name = "Test",
                EmailAddress = "Test@vvpec.com"
            };

            _customerServiceMock.Setup(s => s.GetById(1)).Returns(customer);

            //Act
            var result = _sut.GetById(1);

            //Assert
            _loggerMock.Verify(s => s.LogInformation("Method call GetById"), Times.Once);
            result.Should().Be(customer);
        }

        [Fact]
        public void CustomerController_ShouldPassWhen_CustomerNotExists()
        {
            //Arrange
            _customerServiceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns(() => null);

            //Act
            var result = _sut.GetById(It.IsAny<int>());

            //Assert
            _loggerMock.Verify(s => s.LogInformation("Method call GetById"), Times.Once);
            result.Should().Be(null);
        }
    }
}
