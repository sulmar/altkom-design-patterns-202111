using MediatorPattern.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatorPattern.Requests
{
    public class GetCustomerRequest : IRequest<Customer>
    {
        public GetCustomerRequest(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}
