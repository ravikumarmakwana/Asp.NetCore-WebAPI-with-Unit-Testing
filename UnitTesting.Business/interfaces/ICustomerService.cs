using System.Collections.Generic;
using UnitTesting.API.Entities;

namespace UnitTesting.API.Services.interfaces
{
    public interface ICustomerService
    {
        Customer Add(Customer customer);
        Customer GetById(int id);
        IEnumerable<Customer> GetAll();
        void Remove(int id);
        Customer Update(int id, Customer customer);
    }
}
