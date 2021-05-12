using Moq;
using UnitTesting.API.Entities;
using UnitTesting.API.Repositories.interfaces;
using UnitTesting.API.Services.implementations;
using FluentAssertions;
using Xunit;
using System;

namespace UnitTesting.Test
{
    public class CustomerServiceTest
    {
        private readonly CustomerService _sut;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;

        public CustomerServiceTest()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();

            _sut = new CustomerService(_customerRepositoryMock.Object);
        }

        [Fact]
        public void CustomerService_ShouldPassWhenReturnExistingCustomer()
        {
            //Arrange
            var customer = new Customer()
            {
                Name = "Test",
                Age = 20,
                EmailAddress = "Test@google.com",
                Id = 1
            };

            _customerRepositoryMock.Setup(s => s.GetById(customer.Id))
                .Returns(customer);

            //Act
            var result = _sut.GetById(customer.Id);

            //Assert
            result.Should().Be(customer);
        }

        [Fact]
        public void CustomerService_ShouldFailWhenCustomerNotExists()
        {
            //Arrange
            _customerRepositoryMock.Setup(s => s.GetById(It.IsAny<int>()))
                .Returns(() => null);

            //Act
            var result = _sut.GetById(It.IsAny<int>());

            //Assert
            result.Should().Be(null);
        }

        [Fact]
        public void CustomerService_ShouldPassWhenCustomerAddedSuccessfully()
        {
            //Arrange
            var customerToAdd = new Customer()
            {
                Age = 20,
                Name = "Test",
                EmailAddress = "Test@tarktech.com"
            };

            var addedCustomer = new Customer()
            {
                Id = 1,
                Age = 20,
                Name = "Test",
                EmailAddress = "Test@tarktech.com"
            };

            _customerRepositoryMock.Setup(s => s.Add(customerToAdd))
                .Returns(addedCustomer);

            //Act
            var result = _sut.Add(customerToAdd);

            //Assert
            result.Should().Be(addedCustomer);
        }

        [Fact]
        public void CustomerService_ShouldFailWhenCustomerNotAddedSuccessfully()
        {
            //Arrange
            var customerToAdd = new Customer()
            {
                Age = 3,
                Name = "Test",
                EmailAddress = "Test@tarktech.com"
            };

            _customerRepositoryMock.Setup(s => s.Add(customerToAdd))
                .Returns(() => null);

            //Act
            var result = _sut.Add(customerToAdd);

            //Assert
            result.Should().Be(null);
        }

        [Fact]
        public void CustomerService_ShouldPassWhenCustomerRemoveSuccessfully()
        {
            //Arrange
            int customerId = 1;
            var customer = new Customer()
            {
                Id = 1,
                Age = 20,
                Name = "Test",
                EmailAddress = "Test@gmail.com"
            };

            _customerRepositoryMock.Setup(s => s.GetById(customerId))
                .Returns(customer);

            _customerRepositoryMock.Setup(s => s.Remove(customerId))
                .Verifiable();

            //Act
            Action act = () => _sut.Remove(customerId);

            //Assert
            act.Should().NotThrow<InvalidOperationException>();
        }

        [Fact]
        public void CustomerService_ShouldFailWhenCustomerNotRemoveSuccessfully()
        {
            //Arrange
            int customerId = 1;

            _customerRepositoryMock.Setup(s => s.GetById(customerId))
                .Returns(() => null);

            //Act
            Action act = () => _sut.Remove(customerId);

            //Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Invalid Id");
        }

        [Fact]
        public void CustomerService_ShouldPassWhenCustomerUpdatedSuccessfully()
        {
            //Arrange
            var customerId = 1;
            var customerToUpdate = new Customer()
            {
                Id = 1,
                Name = "Test",
                Age = 20,
                EmailAddress = "Test@outlook.com"
            };

            var updatedCustomer = new Customer()
            {
                Id = 1,
                Name = "TestCustomer",
                Age = 25,
                EmailAddress = "Test@outlook.com"
            };

            _customerRepositoryMock.Setup(s => s.GetById(customerId))
                .Returns(customerToUpdate);

            _customerRepositoryMock.Setup(s => s.Update(customerId, customerToUpdate))
                .Returns(updatedCustomer);

            //Act
            var result = _sut.Update(customerId, customerToUpdate);

            //Assert
            result.Should().Be(updatedCustomer);
        }

        [Fact]
        public void CustomerService_ShouldFailWhenCustomerNotUpdatedSuccessfully()
        {
            //Arrange
            var customerId = 1;
            var customerToUpdate = new Customer()
            {
                Id = 1,
                Name = "Test",
                Age = 20,
                EmailAddress = "Test@outlook.com"
            };

            //Act
            _customerRepositoryMock.Setup(s=>s.GetById(customerId))
                .Returns(() => null);

            //Assert
            _customerRepositoryMock.Verify(s => s.GetById(customerId), Times.Once);
            _customerRepositoryMock.Verify(s => s.Update(customerId,customerToUpdate), Times.Never);
        }
    }
}
