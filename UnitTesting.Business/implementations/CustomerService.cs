using System.Collections.Generic;
using UnitTesting.Business.interfaces;
using UnitTesting.Data.interfaces;
using UnitTesting.Entities;

namespace UnitTesting.Business.implementations
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
            _customerRepository.Remove(id);
        }

        public Customer Update(int id, Customer customer)
        {
            return _customerRepository.Update(id, customer);
        }
    }
}
