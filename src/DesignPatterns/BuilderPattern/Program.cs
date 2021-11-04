using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace BuilderPattern
{
    // Abstract Builder
    public interface ISalesReportBuilder
    {
        void AddHeader();
        void AddSectionByGender();
        void AddSectionByProduct();

        void AddFooter();

        SalesReport Build();
    }
  
    // Concrete Builder
    public class SalesReportBuilder : ISalesReportBuilder
    {
        // Product
        private SalesReport salesReport;

        private IEnumerable<Order> orders;

        public SalesReportBuilder(IEnumerable<Order> orders)
        {
            this.orders = orders;

            salesReport = new SalesReport();            
        }

        public void AddFooter()
        {
            salesReport.Footer = $"Wydrukowano z programu ABC w dn. {DateTime.Now}";
        }

        public void AddHeader()
        {
            salesReport.Title = "Raport sprzedaży";
            salesReport.CreateDate = DateTime.Now;
            salesReport.TotalSalesAmount = orders.Sum(s => s.Amount);
        }

        public void AddSectionByGender()
        {
            salesReport.GenderDetails = orders
               .GroupBy(o => o.Customer.Gender)
               .Select(g => new GenderReportDetail(
                           g.Key,
                           g.Sum(x => x.Details.Sum(d => d.Quantity)),
                           g.Sum(x => x.Details.Sum(d => d.LineTotal))));
        }

        public void AddSectionByProduct()
        {
            salesReport.ProductDetails = orders
             .SelectMany(o => o.Details)
             .GroupBy(o => o.Product)
             .Select(g => new ProductReportDetail(g.Key, g.Sum(p => p.Quantity), g.Sum(p => p.LineTotal)));
        }


        // Product
        public SalesReport Build()
        {           

            return salesReport;
        }

    }

    public class LazySalesReportBuilder : ISalesReportBuilder
    {        
        private IEnumerable<Order> orders;

        private bool hasHeader;
        private bool hasSectionByGender;
        private bool hasSectionByProduct;


        public LazySalesReportBuilder(IEnumerable<Order> orders)
        {
            this.orders = orders;
        }

        public void AddFooter()
        {
            throw new NotImplementedException();
        }

        public void AddHeader()
        {
            hasHeader = true;
        }

        public void AddSectionByGender()
        {
            hasSectionByGender = true;
        }

        public void AddSectionByProduct()
        {
            hasSectionByProduct = true;          
        }


        // Product
        public SalesReport Build()
        {
            SalesReport salesReport = new SalesReport();

            if (hasHeader)
            {
                salesReport.Title = "Raport sprzedaży";
                salesReport.CreateDate = DateTime.Now;
                salesReport.TotalSalesAmount = orders.Sum(s => s.Amount);
            }

            if (hasSectionByGender)
            {
                salesReport.GenderDetails = orders
                  .GroupBy(o => o.Customer.Gender)
                  .Select(g => new GenderReportDetail(
                              g.Key,
                              g.Sum(x => x.Details.Sum(d => d.Quantity)),
                              g.Sum(x => x.Details.Sum(d => d.LineTotal))));
            }

            if (hasSectionByProduct)
            {
                salesReport.ProductDetails = orders
                   .SelectMany(o => o.Details)
                   .GroupBy(o => o.Product)
                   .Select(g => new ProductReportDetail(g.Key, g.Sum(p => p.Quantity), g.Sum(p => p.LineTotal)));
            }

            return salesReport;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Builder Pattern!");

            // StringBuilderTest();

            // SalesReportTest();

            FluentPhoneTest();
        }

        private static void StringBuilderTest()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Hello World!");
            stringBuilder.AppendLine("Footer");

            string product = stringBuilder.ToString(); // Build
        }

        private static void SalesReportTest()
        {
            FakeOrdersService ordersService = new FakeOrdersService();
            IEnumerable<Order> orders = ordersService.Get();

            SalesReportBuilder salesReportBuilder = new SalesReportBuilder(orders);

            bool byProduct = true;
            bool byGender = true;


            salesReportBuilder.AddHeader();
            
            if (byGender)
                salesReportBuilder.AddSectionByGender();

            if (byProduct)
                salesReportBuilder.AddSectionByProduct();

            salesReportBuilder.AddFooter();


            // Product
            SalesReport salesReport = salesReportBuilder.Build();

            Console.WriteLine(salesReport);

        }

        private static void PhoneTest()
        {
            Phone phone = new Phone();
            phone.Call("555999123", "555000321", ".NET Design Patterns");
        }

        private static void FluentPhoneTest()
        {
            string[] numbers = new string[] { "5550666", "5550666", "5550666", };

            FluentPhone.Pickup()
                .From("555111333")                
                .To("555000321")
                .To("555999111")
                .To("555111000")      
                .To(numbers)
                .To("555000111", "55522200", "555333000")
                .WithSubject(".NET Design Patterns")
                .Call();    // Fluent API

            // ls > grep > grep > files.txt
        }
    }

    #region FluentPhone

    public interface IFrom
    {
        ITo From(string number);
    }

    public interface ITo
    {
        IToOrCallOrSubject To(string number);

        IToOrCallOrSubject To(params string[] numbers);
    }

    public interface IToOrCallOrSubject : ICall, ITo, ISubject
    {

    }

    public interface ISubject
    {
        ICall WithSubject(string subject);
    }

    public interface ICall
    {
        void Call();
    }

    // Abstract Builder
    public interface IFluentPhone : IFrom, ITo, ICall, IToOrCallOrSubject, ISubject
    {

    }

    // Concrete Builder
    public class FluentPhone : IFluentPhone
    {
        private string from;
        private ICollection<string> tos;
        private string subject;

        protected FluentPhone()
        {
            tos = Enumerable.Empty<string>().ToList();
        }

        public static IFrom Pickup()
        {
            return new FluentPhone();
        }

        public ITo From(string number)
        {
            this.from = number;

            return this;
        }

        public IToOrCallOrSubject To(string number)
        {
            this.tos.Add(number);

            return this;
        }


        public ICall WithSubject(string subject)
        {
            this.subject = subject;

            return this;
        }

        public void Call() // Build
        {
            string to = string.Join(",", tos);

            if (string.IsNullOrEmpty(subject))
                Console.WriteLine($"[{from}] {to}");
            else
                Console.WriteLine($"[{from}] {to} {subject}");
        }

        public IToOrCallOrSubject To(string[] numbers)
        {
            foreach (var number in numbers)
            {
                To(number);
            }

            return this;
        }
    }


    #endregion


    #region SalesReport

    public class FakeOrdersService
    {
        private readonly IList<Product> products;
        private readonly IList<Customer> customers;

        public FakeOrdersService()
            : this(CreateProducts(), CreateCustomers())
        {

        }

        public FakeOrdersService(IList<Product> products, IList<Customer> customers)
        {
            this.products = products;
            this.customers = customers;
        }

        private static IList<Customer> CreateCustomers()
        {
            return new List<Customer>
            {
                 new Customer("Anna", "Kowalska"),
                 new Customer("Jan", "Nowak"),
                 new Customer("John", "Smith"),
            };

        }

        private static IList<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product(1, "Książka C#", unitPrice: 100m),
                new Product(2, "Książka Praktyczne Wzorce projektowe w C#", unitPrice: 150m),
                new Product(3, "Zakładka do książki", unitPrice: 10m),
            };
        }

        public IEnumerable<Order> Get()
        {
            Order order1 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[0]);
            order1.AddDetail(products[0], 2);
            order1.AddDetail(products[1], 2);
            order1.AddDetail(products[2], 3);

            yield return order1;

            Order order2 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[1]);
            order2.AddDetail(products[0], 2);
            order2.AddDetail(products[1], 4);

            yield return order2;

            Order order3 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[2]);
            order2.AddDetail(products[0], 2);
            order2.AddDetail(products[2], 5);

            yield return order3;


        }
    }

  
    public class SalesReport
    {
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public string Footer { get; set; }

        public IEnumerable<ProductReportDetail> ProductDetails { get; set; }
        public IEnumerable<GenderReportDetail> GenderDetails { get; set; }


        public override string ToString()
        {
            string output = string.Empty;

            output += "------------------------------\n";

            output += $"{Title} {CreateDate}\n";
            output += $"Total Sales Amount: {TotalSalesAmount:c2}\n";

            output += "------------------------------\n";

            output += "Total By Products:\n";
            foreach (var detail in ProductDetails)
            {
                output += $"- {detail.Product.Name} {detail.Quantity} {detail.TotalAmount:c2}\n";
            }
            output += "Total By Gender:\n";
            foreach (var detail in GenderDetails)
            {
                output += $"- {detail.Gender} {detail.Quantity} {detail.TotalAmount:c2}\n";
            }

            return output;
        }
    }

    public class ProductReportDetail
    {
        public ProductReportDetail(Product product, int quantity, decimal totalAmount)
        {
            Product = product;
            Quantity = quantity;
            TotalAmount = totalAmount;
        }

        public Product Product { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
    }

    public class GenderReportDetail
    {
        public GenderReportDetail(Gender gender, int quantity, decimal totalAmount)
        {
            Gender = gender;
            Quantity = quantity;
            TotalAmount = totalAmount;
        }

        public Gender Gender { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
    }


    public class Order
    {
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public decimal Amount => Details.Sum(p => p.LineTotal);

        public ICollection<OrderDetail> Details = new Collection<OrderDetail>();

        public void AddDetail(Product product, int quantity = 1)
        {
            OrderDetail detail = new OrderDetail(product, quantity);

            this.Details.Add(detail);
        }

        public Order(DateTime orderDate, Customer customer)
        {
            OrderDate = orderDate;
            Customer = customer;
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
    }

    public class OrderDetail
    {
        public OrderDetail(Product product, int quantity = 1)
        {
            Product = product;
            Quantity = quantity;

            UnitPrice = product.UnitPrice;
        }

        public int Id { get; set; }
        public Product Product { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
    }

    public class Customer
    {
        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            if (firstName.EndsWith("a"))
            {
                Gender = Gender.Female;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }

    }

    public enum Gender
    {
        Male,
        Female
    }

    #endregion

    #region Phone

    public class Phone
    {
        public void Call(string from, string to, string subject)
        {
            Console.WriteLine($"Calling from {from} to {to} with subject {subject}");
        }

        public void Call(string from, string to)
        {
            Console.WriteLine($"Calling from {from} to {to}");
        }

        public void Call(string from, IEnumerable<string> tos, string subject)
        {
            foreach (var to in tos)
            {
                Call(from, to, subject);
            }
        }
    }

    #endregion


}