using MediatorPattern.Events;
using MediatorPattern.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatorPattern.Handlers
{

    public class SendMessageHandler : INotificationHandler<AddCustomerEvent>
    {
        public Task Handle(AddCustomerEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Send email to {notification.Customer.Email}");

            return Task.CompletedTask;
        }
    }
}
