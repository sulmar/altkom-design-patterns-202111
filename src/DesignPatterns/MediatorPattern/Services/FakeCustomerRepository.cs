using MediatorPattern.IServices;
using MediatorPattern.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MediatorPattern.Services
{
    public class FakeCustomerRepository : ICustomerRepository
    {
        private readonly IDictionary<int, Customer> customers = new Dictionary<int, Customer>();

        public void Add(Customer customer)
        {
            if (customers.Keys.Any())
            {
                customer.Id = customers.Keys.Max() + 1;
            }
            else
                customer.Id = 1;

            customers.Add(customer.Id, customer);

        }


        public ICollection<Customer> Get()
        {
            return customers.Values.ToList();
        }

        public Customer Get(int id)
        {
            customers.TryGetValue(id, out Customer customer);
            return customer;
        }
    }
}