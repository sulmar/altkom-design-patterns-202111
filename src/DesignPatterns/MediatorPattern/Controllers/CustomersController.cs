using MediatorPattern.Events;
using MediatorPattern.IServices;
using MediatorPattern.Models;
using MediatorPattern.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatorPattern.Controllers
{
    // dotnet add package MediatR


    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator mediator;

        public CustomersController(IMediator mediator)
        {
            this.mediator = mediator;
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
            // Fire and Forgot
            mediator.Publish(new AddCustomerEvent(customer));

            return Ok();
        }

        // GET https://localhost:5001/api/customers HTTP/1.1
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            // Request
            // mediator.Send();

            //var customers = customerRepository.Get();
            //return Ok(customers);

            throw new NotImplementedException();
        }

        // GET https://localhost:5001/api/customers/1 HTTP/1.1
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            Customer customer = await mediator.Send(new GetCustomerRequest(id));

            return Ok(customer);
        }

    }
}
