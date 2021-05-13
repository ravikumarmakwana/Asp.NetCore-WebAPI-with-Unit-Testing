using Moq;
using FluentAssertions;
using Xunit;
using System;
using UnitTesting.Data.interfaces;
using UnitTesting.Business.implementations;
using UnitTesting.Entities;

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
        public void GetById_ShouldPassWhenReturnExistingCustomer()
        {
            int customerId = 1;
            var customer = new Customer()
            {
                Name = "Test",
                Age = 20,
                EmailAddress = "Test@google.com",
                Id = 1
            };

            _customerRepositoryMock.Setup(s => s.GetById(customerId))
                .Returns(customer);

            var result = _sut.GetById(customerId);

            result.Should().Be(customer);
        }

        [Fact]
        public void GetById_ShouldFailWhenCustomerNotExists()
        {
            _customerRepositoryMock.Setup(s => s.GetById(It.IsAny<int>()))
                .Throws(new InvalidOperationException("Invalid Id"));

            Action result = () => _sut.GetById(It.IsAny<int>());

            result.Should().Throw<InvalidOperationException>()
                .WithMessage("Invalid Id");
        }

        [Fact]
        public void Add_ShouldPassWhenCustomerAddedSuccessfully()
        {
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

            var result = _sut.Add(customerToAdd);

            result.Should().Be(addedCustomer);
        }

        [Fact]
        public void Add_ShouldFailWhenCustomerNotAddedSuccessfully()
        {
            var customerToAdd = new Customer()
            {
                Age = 3,
                Name = "Test",
                EmailAddress = "Test@tarktech.com"
            };

            _customerRepositoryMock.Setup(s => s.Add(customerToAdd))
                .Throws(new InvalidOperationException("Customer not added successfully"));

            Action result = () => _sut.Add(customerToAdd);

            result.Should().Throw<InvalidOperationException>()
                .WithMessage("Customer not added successfully");
        }

        [Fact]
        public void Remove_ShouldPassWhenCustomerRemoveSuccessfully()
        {
            int customerId = 1;

            var customer = new Customer()
            {
                Id = 1,
                Age = 20,
                Name = "Test",
                EmailAddress = "Test@gmail.com"
            };

            Action act = () => _sut.Remove(customerId);

            act.Should().NotThrow<InvalidOperationException>();
            _customerRepositoryMock.Verify(s => s.Remove(customerId), Times.Once);
        }

        [Fact]
        public void Remove_ShouldFailWhenCustomerNotRemoveSuccessfully()
        {
            int customerId = 1;
            _customerRepositoryMock.Setup(s => s.Remove(customerId))
                .Throws(new InvalidOperationException("Invalid Id"));

            Action result = () => _sut.Remove(customerId);

            result.Should().Throw<InvalidOperationException>()
                .WithMessage("Invalid Id");
        }

        [Fact]
        public void Update_ShouldPassWhenCustomerShouldUpdateSuccessfully()
        {
            var customerId = 1;
            var customerToUpdate = new Customer()
            {
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

            _customerRepositoryMock.Setup(s => s.Update(customerId, customerToUpdate))
                .Returns(updatedCustomer);

            var result = _sut.Update(customerId, customerToUpdate);

            result.Should().Be(updatedCustomer);
        }

        [Fact]
        public void Update_ShouldFailWhenCustomerShouldNotUpdateSuccessfully()
        {
            var customerId = 1;
            var customerToUpdate = new Customer()
            {
                Name = "Test",
                Age = 20,
                EmailAddress = "Test@outlook.com"
            };
           
            _customerRepositoryMock.Setup(s => s.Update(customerId, customerToUpdate))
                .Throws(new InvalidOperationException("Invalid Id"));

            Action result = () => _sut.Update(customerId, customerToUpdate);

            result.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Invalid Id");
        }
    }
}
