using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace ProxyPattern
{
    // Abstract Subject
    public interface ICustomer
    {
        string Name { get; set; }

        void DoWork();
    }

    // Real Subject
    public class Customer : ICustomer
    {
        public virtual string Name { get; set; }

        public virtual void DoWork()
        {
            Console.WriteLine($"working...");
        }
    }

    // Wariant klasowy
    // Proxy
    public class CustomerClassProxy : Customer, ICustomer
    {
        public override string Name
        {
            get => base.Name; set
            {
                base.Name = value;
                Console.WriteLine($"[{DateTime.Now}] Changed name {Name}");
            }
        }

        public override void DoWork()
        {
            Console.WriteLine($"[{DateTime.Now}] Do Work");

            base.DoWork();
        }
    }

    // Proxy
    // Wariant obiektowy
    public class CustomerProxy : ICustomer
    {
        // Real Subject
        private Customer customer; 

        public CustomerProxy(Customer customer)
        {
            this.customer = customer;
        }

        public string Name
        {
            get
            {
                return customer.Name;
            }
            set
            {
                customer.Name = value;

                Console.WriteLine($"[{DateTime.Now}] Changed name {Name}");
            }
        }

        public void DoWork()
        {
            Console.WriteLine($"[{DateTime.Now}] Do Work");

            customer.DoWork();
        }
    }

    public class ProductRepositoryProxy : IProductRepository
    {


        public Product Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Proxy Pattern!");

            // ICustomer customer = new CustomerProxy(new Customer());

            ICustomer customer = new CustomerClassProxy();
            customer.Name = "John";
            customer.DoWork();


            GetProductTest();

            //SaveProductTest();

        }


        private static void GetProductTest()
        {
            IProductRepository productRepository = new CacheProductRepository(new DbProductRepository());

            while (true)
            {

                Console.Write("Podaj id produktu: ");

                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    Product product = productRepository.Get(productId);

                    Console.WriteLine($"{product.Id} {product.Name} {product.UnitPrice:C2}");
                }
            }


        }

        private static void SaveProductTest()
        {
            ProductsDbContext context = new ProductsDbContext();

            Product product = new Product(1, "Design Patterns w C#", 150m);

            context.Add(product);

            product.UnitPrice = 99m;

            context.MarkAsChanged();

            context.SaveChanges();
        }
    }

    #region Models
    public class Product
    {
        public Product(int id, string name, decimal unitPrice)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
        }

        public int Id { get; set; }
        public virtual string Name { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual byte[] Photo { get; set; }
    }

    public class ProductProxy : Product
    {
        public bool HasChanged { get; private set; }

        public ProductProxy(int id, string name, decimal unitPrice) : base(id, name, unitPrice)
        {
            HasChanged = false;
        }

        public override string Name
        {
            get => base.Name; 
            set
            {
                if (base.Name != null)
                {
                    base.Name = value;
                    HasChanged = true;
                }
            }
        }

        public override byte[] Photo
        {
            get
            {
                // SELECT Photo from dbo.Products WHERE ProductId = {Id}
                return base.Photo;
            }

            set => base.Photo = value;
        }
    }


    public interface IProductRepository
    {
        Product Get(int id);
    }

    //  Proxy Subject
    public class CacheProductRepository : IProductRepository
    {
        // Real subject

        private readonly IProductRepository productRepository;

        public CacheProductRepository()
        {
            products = new Collection<Product>();
        }

        public CacheProductRepository(IProductRepository productRepository)
            : this()
        {
            this.productRepository = productRepository;
        }

        private ICollection<Product> products;



        public void Add(Product product)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Add product {product.Id} to cache");
            Console.ResetColor();
            products.Add(product);
        }

        public Product Get(int id)
        {

            // Before
            Product product = products.SingleOrDefault(p => p.Id == id);

            if (product != null)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Get product {id} from cache");
                Console.ResetColor();
                return product;
            }
            else
            {
                // Real subject
                product = productRepository.Get(id);


                // After
                products.Add(product);
                return product;
            }

        }

    }

    // Real Subject
    public class DbProductRepository : IProductRepository
    {
        private ICollection<Product> products;

        public DbProductRepository()
        {
            products = new Collection<Product>()
            {
                new Product(1, "Product 1", 10),
                new Product(2, "Product 2", 10),
                new Product(3, "Product 3", 10),
            };
        }

        public Product Get(int id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"Get product {id} from database");
            Console.ResetColor();
            return products.SingleOrDefault(p => p.Id == id);
        }
    }

    public class ProductsDbContext
    {
        private Product product;
        private bool changed;

        public void Add(Product product)
        {
            this.product = product;
        }

        public Product Get()
        {
            return product;
        }

        public void SaveChanges()
        {
            if (changed)
            {
                Console.WriteLine($"UPDATE dbo.Products SET UnitPrice = {product.UnitPrice} WHERE ProductId = {product.Id}");
            }
        }

        public void MarkAsChanged()
        {
            changed = true;
        }
    }

    #endregion
}
