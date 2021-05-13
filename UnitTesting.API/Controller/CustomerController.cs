using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UnitTesting.Business.interfaces;
using UnitTesting.Core;
using UnitTesting.Entities;
using UnitTesting.Validations;

namespace UnitTesting.API.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly CustomerValidation _validationRules;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger logger, CustomerValidation validationRules, ICustomerService customerService)
        {
            _validationRules = validationRules;
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Customer> GetAll()
        {
            _logger.LogInformation("Method call GetAll");
            return _customerService.GetAll();
        }

        [HttpGet("{id}")]
        public Customer GetById(int id)
        {
            _logger.LogInformation("Method call GetById");
            return _customerService.GetById(id);
        }

        [HttpPost]
        public Customer Add(Customer customer)
        {
            var result = _validationRules.Validate(customer);

            if (!result.IsValid)
                throw new InvalidOperationException(result.ToString());

            var addedCustomer = _customerService.Add(customer);

            _logger.LogInformation("Customer Added Successfully");

            return addedCustomer;
        }

        [HttpPut("{id}")]
        public Customer Update(int id, Customer customer)
        {
            var result = _validationRules.Validate(customer);

            if (!result.IsValid)
                throw new InvalidOperationException(result.ToString());

            var updatedCustomer = _customerService.Update(id, customer);

            _logger.LogInformation("Customer Updated Successfully");

            return updatedCustomer;
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            _customerService.Remove(id);
            _logger.LogInformation("Customer Deleted Successfully");

            return Ok();
        }
    }
}
