using System.Collections.Generic;
using UnitTesting.API.Contexts;
using UnitTesting.API.Entities;
using UnitTesting.API.Repositories.interfaces;
using System.Linq;
using System;

namespace UnitTesting.API.Repositories.implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Customer Add(Customer customer)
        {
            _context.Add(customer);
            _context.SaveChanges();

            return customer;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers;
        }

        public Customer GetById(int id)
        {
            return _context.Customers.FirstOrDefault(s => s.Id == id);
        }

        public void Remove(int id)
        {
            var customer = GetById(id);

            if (customer == null)
                throw new InvalidOperationException("Invalid Id");

            _context.Remove(customer);
            _context.SaveChanges();
        }

        public Customer Update(int id, Customer customer)
        {
            var customerToUpdate = GetById(id);

            if (customerToUpdate == null)
                throw new InvalidOperationException("Invalid Id");

            customerToUpdate.Age = customer.Age;
            customerToUpdate.Name = customer.Name;
            customerToUpdate.EmailAddress = customer.EmailAddress;

            _context.Update(customerToUpdate);
            _context.SaveChanges();

            return customerToUpdate;
        }
    }
}
