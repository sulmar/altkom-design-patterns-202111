using MediatorPattern.IServices;
using MediatorPattern.Models;
using MediatorPattern.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatorPattern.RequestHandlers
{
    public class GetCustomerRequestHandler : IRequestHandler<GetCustomerRequest, Customer>
    {
        private readonly ICustomerRepository customerRepository;

        public GetCustomerRequestHandler(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public Task<Customer> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
        {
            Customer customer = customerRepository.Get(request.Id);

            return Task.FromResult(customer);
        }
    }
}
