using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ChainOfResponsibilityPattern
{
    public interface IHandler
    {
        string Handle(decimal request);
        IHandler SetHandler(IHandler next);

        Task<string> HandleAsync(decimal request);
    }

    // Abstract handler
    public class AbstractHandler : IHandler
    {
        private IHandler next;

        public virtual string Handle(decimal request)
        {
            if (this.next != null)
            {
                return this.next.Handle(request);
            }
            else
            {
                return null;
            }
        }

        public virtual Task<string> HandleAsync(decimal request)
        {
            return Task.Run(() => Handle(request));
        }

        public IHandler SetHandler(IHandler next)
        {
            this.next = next;

            return next;
        }
    }

    // Concrete Handler
    public class DeveloperHandler : AbstractHandler
    {
        public override string Handle(decimal request)
        {
            if (request < 100)
            {
                return "Sign by developer";
            }
            else
                return base.Handle(request);
        }
    }

    public class BossHandler : AbstractHandler
    {
        public bool Enabled { get; set; }

        public override string Handle(decimal request)
        {
            if (Enabled && request < 5000)
            {
                return "Sign by boss";
            }
            else
                return base.Handle(request);
        }

        
        
    }

    public class SecretaryHandler : AbstractHandler
    {
        public override string Handle(decimal request)
        {
            Console.WriteLine($"LOG [{DateTime.Now}] {request}");

            string response = base.Handle(request);

            Console.WriteLine($"LOG [{DateTime.Now}] {response}");

            return response;
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Chain of Responsibility Pattern!");

            // CacheProductTest();
            
            IHandler developerHandler = new DeveloperHandler();
            IHandler bossHandler = new BossHandler() { Enabled = true };

            IHandler handler = developerHandler
                    .SetHandler(new SecretaryHandler())
                        .SetHandler(bossHandler);

            decimal amount = 200;

            string result = developerHandler.Handle(amount);

            Console.WriteLine(result);

        }

        private static void CacheProductTest()
        {
            CacheProductService cacheProductService = new CacheProductService();
            IProductService productService = new DbProductService();

            int productId = 1;

            for (int i = 0; i < 3; i++)
            {
                Product product = cacheProductService.Get(productId);

                if (product == null)
                {
                    product = productService.Get(productId);

                    cacheProductService.Set(productId, product);
                }

                Console.WriteLine(product);
            }
        }
    }

    public class Product
    {
        public Product(int id, string name, decimal unitPrice)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }

        public override string ToString()
        {
            return $"{Id} {Name} {UnitPrice:C2}";
        }
    }

    public interface IProductService
    {
        Product Get(int id);
    }

    public class CacheProductService
    {
        private readonly IDictionary<int, Product> products = new Dictionary<int, Product>();

        public void Set(int id, Product product)
        {
            products.TryAdd(id, product);
        }

        public Product Get(int id)
        {
            Console.WriteLine($"Get product Id={id} from cache");

            products.TryGetValue(id, out Product product);

            return product;
        }


    }

    public class DbProductService : IProductService
    {
        private readonly ICollection<Product> products;

        public DbProductService()
        {
            products = new Collection<Product>
            {
                new Product(1, "Książka C#", 100m),
                new Product(2, "Książka Praktyczne Wzorce projektowe w C#", unitPrice: 150m),
                new Product(3, "Zakładka do książki", unitPrice: 10m),
            };
        }

        public Product Get(int id)
        {
            Console.WriteLine($"Get product Id={id} from database");
            return products.SingleOrDefault(p => p.Id == id);
        }
    }

}
