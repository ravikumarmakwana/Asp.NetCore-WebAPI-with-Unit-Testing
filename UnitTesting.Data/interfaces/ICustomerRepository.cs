using System.Collections.Generic;
using UnitTesting.Entities;

namespace UnitTesting.Data.interfaces
{
    public interface ICustomerRepository
    {
        Customer Add(Customer customer);
        Customer GetById(int id);
        IEnumerable<Customer> GetAll();
        void Remove(int id);
        Customer Update(int id, Customer customer);
    }
}
