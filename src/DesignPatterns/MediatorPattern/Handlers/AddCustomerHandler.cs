using MediatorPattern.Events;
using MediatorPattern.IServices;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MediatorPattern.Handlers
{
    public class AddCustomerHandler : INotificationHandler<AddCustomerEvent>
    {
        private readonly ICustomerRepository customerRepository;

        public AddCustomerHandler(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public Task Handle(AddCustomerEvent notification, CancellationToken cancellationToken)
        {
            customerRepository.Add(notification.Customer);

            return Task.CompletedTask;
            
        }
    }
}
