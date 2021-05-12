using System;
using System.Collections.Generic;
using UnitTesting.API.Entities;
using UnitTesting.API.Repositories.interfaces;
using UnitTesting.API.Services.interfaces;

namespace UnitTesting.API.Services.implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Customer Add(Customer customer)
        {
            return _customerRepository.Add(customer);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public Customer GetById(int id)
        {
            return _customerRepository.GetById(id);
        }

        public void Remove(int id)
        {
            var customer = _customerRepository.GetById(id);

            if (customer == null)
                throw new InvalidOperationException("Invalid Id");

            _customerRepository.Remove(id);
        }

        public Customer Update(int id, Customer customer)
        {
            var customerToUpdate = _customerRepository.GetById(id);

            if (customerToUpdate == null)
                throw new InvalidOperationException("Invalid Id");

            return _customerRepository.Update(id, customer);
        }
    }
}
