using MediatorPattern.IServices;
using MediatorPattern.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatorPattern.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMessageService messageService;

        public CustomersController(ICustomerRepository customerRepository, IMessageService messageService)
        {
            this.customerRepository = customerRepository;
            this.messageService = messageService;
        }

        /*
            POST https://localhost:5001/api/customers HTTP/1.1
            content-type: application/json

            {
                "FirstName": "John",
                "LastName": "Smith",
                "Email": "john.smith@domain.com"
            }
  
        */

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            customerRepository.Add(customer);

            messageService.Send(customer.Email, $"Welcome {customer.FullName}");

            return Ok();
        }

        // GET https://localhost:5001/api/customers HTTP/1.1
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            var customers = customerRepository.Get();
            return Ok(customers);
        }

        // GET https://localhost:5001/api/customers/1 HTTP/1.1
        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            var customers = customerRepository.Get(id);
            return Ok(customers);
        }

    }
}
