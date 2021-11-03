using MediatorPattern.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatorPattern.IServices
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);

        ICollection<Customer> Get();
        Customer Get(int id);
    }
}
