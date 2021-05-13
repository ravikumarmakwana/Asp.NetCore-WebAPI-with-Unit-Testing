using System;
using FluentAssertions;
using Moq;
using UnitTesting.API.Controller;
using UnitTesting.Business.interfaces;
using UnitTesting.Core;
using UnitTesting.Entities;
using UnitTesting.Validations;
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
        public void Add_ShouldPassWhenCustomerAddedSuccessfully()
        {
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

            var validationResult = _customerValidation.Validate(customerToAdd);

            _customerServiceMock.Setup(s => s.Add(customerToAdd))
                .Returns(addedCustomer);

            var result = _sut.Add(customerToAdd);

            result.Should().Be(addedCustomer);
            validationResult.IsValid.Should().BeTrue();
            _loggerMock.Verify(s => s.LogInformation("Customer Added Successfully"), Times.Once);
        }

        [Fact]
        public void Add_ShouldFailWhenCustomerNotAddedSuccessfully()
        {
            var customerToAdd = new Customer()
            {
                Age = 19,
                Name = "Test",
                EmailAddress = "Test@tarktech.com"
            };

            var validationResult = _customerValidation.Validate(customerToAdd);

            _customerServiceMock.Setup(s => s.Add(customerToAdd))
                .Returns(() => null);

            var result = _sut.Add(customerToAdd);

            validationResult.IsValid.Should().BeTrue();
            _loggerMock.Verify(s => s.LogInformation("Customer Added Successfully"), Times.Once);
            result.Should().Be(null);
        }

        [Fact]
        public void Add_ShouldFailWhenCustomerValidationShouldFail()
        {
            var customerToAdd = new Customer()
            {
                Age = 10,
                Name = "Test",
                EmailAddress = "Test@tarktech.com"
            };

            var validationResult = _customerValidation.Validate(customerToAdd);

            Action result = () => _sut.Add(customerToAdd);

            validationResult.IsValid.Should().BeFalse();
            result.Should().Throw<InvalidOperationException>()
                .WithMessage(CustomerValidation.AgeMustBeGreaterThanOrEqualTo18);
            _loggerMock.Verify(s => s.LogInformation("Customer Added Successfully"), Times.Never);
            _customerServiceMock.Verify(s => s.Add(customerToAdd), Times.Never);
        }

        [Fact]
        public void GetById_ShouldPassWhenReturnExistingCustomer()
        {
            var customerId = 1;
            var customer = new Customer()
            {
                Id = 1,
                Age = 100,
                Name = "Test",
                EmailAddress = "Test@vvpec.com"
            };

            _customerServiceMock.Setup(s => s.GetById(customerId))
                .Returns(customer);

            var result = _sut.GetById(customerId);

            _loggerMock.Verify(s => s.LogInformation("Method call GetById"), Times.Once);
            result.Should().Be(customer);
        }

        [Fact]
        public void GetById_ShouldFailWhenCustomerNotExists()
        {
            _customerServiceMock.Setup(s => s.GetById(It.IsAny<int>()))
                .Throws(new InvalidOperationException("Invalid Id"));

            Action result = () => _sut.GetById(It.IsAny<int>());

            result.Should().Throw<InvalidOperationException>()
                .WithMessage("Invalid Id");
            _loggerMock.Verify(s => s.LogInformation("Method call GetById"), Times.Once);
        }

        [Fact]
        public void Update_ShouldPassWhenCustomerUpdatedSuccessfully()
        {
            int customerId = 1;
            var customerToUpdate = new Customer()
            {
                Name = "Test Customer",
                EmailAddress = "Test@gmail.com",
                Age = 30
            };

            var updatedCustomer = new Customer()
            {
                Id = 1,
                Name = "Test Customer",
                EmailAddress = "Test@gmail.com",
                Age = 30
            };

            var validationResult = _customerValidation.Validate(customerToUpdate);

            _customerServiceMock.Setup(s => s.Update(customerId, customerToUpdate))
                .Returns(updatedCustomer);

            var result = _sut.Update(customerId, customerToUpdate);

            validationResult.IsValid.Should().BeTrue();
            result.Should().Be(updatedCustomer);
            _loggerMock.Verify(s => s.LogInformation("Customer Updated Successfully"), Times.Once);
        }

        [Fact]
        public void Update_ShouldFailWhenCustomerNotUpdatedSuccessfullyWhenValidationFail()
        {
            int customerId = 1;
            var customerToUpdate = new Customer()
            {
                Name = "Test Customer",
                EmailAddress = "Test@gmail.com",
                Age = 10
            };

            var validationResult = _customerValidation.Validate(customerToUpdate);

            Action result = () => _sut.Update(customerId, customerToUpdate);

            result.Should().Throw<InvalidOperationException>()
                .WithMessage(CustomerValidation.AgeMustBeGreaterThanOrEqualTo18);
            validationResult.IsValid.Should().BeFalse();
            _customerServiceMock.Verify(s => s.Update(customerId, customerToUpdate), Times.Never);
            _loggerMock.Verify(s => s.LogInformation("Customer Updated Successfully"), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWhenCustomerNotUpdatedSuccessfullyWhenCustomerNotFound()
        {
            int customerId = 1;
            var customerToUpdate = new Customer()
            {
                Name = "Test Customer",
                EmailAddress = "Test@gmail.com",
                Age = 40
            };

            var validationResult = _customerValidation.Validate(customerToUpdate);

            _customerServiceMock.Setup(s => s.Update(customerId, customerToUpdate))
                .Throws(new InvalidOperationException("Invalid Id"));

            Action result = () => _sut.Update(customerId, customerToUpdate);

            validationResult.IsValid.Should().BeTrue();
            result.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Invalid Id");
            _loggerMock.Verify(s => s.LogInformation("Customer Updated Successfully"), Times.Never);
        }

        [Fact]
        public void Remove_ShouldPassWhenCustomerRemoveSuccessfully()
        {
            int customerId = 1;

            Action result = () => _sut.Remove(customerId);

            result.Should().NotThrow<InvalidOperationException>();
            _customerServiceMock.Verify(s => s.Remove(customerId), Times.Once);
            _loggerMock.Verify(s => s.LogInformation("Customer Deleted Successfully"), Times.Once);
        }

        [Fact]
        public void Remove_ShouldFailWhenCustomerNotRemoveSuccessfully()
        {
            int customerId = 1;

            _customerServiceMock.Setup(s => s.Remove(customerId))
                .Throws(new InvalidOperationException("Invalid Id"));

            Action result = () => _sut.Remove(customerId);

            result.Should().Throw<InvalidOperationException>()
                .WithMessage("Invalid Id");
            _loggerMock.Verify(s => s.LogInformation("Customer Deleted Successfully"), Times.Never);
        }
    }
}
