using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace StrategyPattern
{

    public class OrderCalculatorBuilder
    {
        private ICanDiscountStrategy canDiscountStrategy;
        private IGetDiscountStrategy getDiscountStrategy;

        public void AddCanDiscountStrategy(ICanDiscountStrategy canDiscountStrategy)
        {
            this.canDiscountStrategy = canDiscountStrategy;
        }

        public void AddGetDiscountStrategy(IGetDiscountStrategy getDiscountStrategy)
        {
            this.getDiscountStrategy = getDiscountStrategy;
        }

        public SmartOrderCalculator Build()
        {
            return new SmartOrderCalculator(canDiscountStrategy, getDiscountStrategy);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Strategy Pattern!");

            HappyHoursOrderCalculatorTest();

            HappyHoursPercentageSmartOrderCalculatorTest();


        }

        private static void HappyHoursPercentageSmartOrderCalculatorTest()
        {
            Customer customer = new Customer("Anna", "Kowalska");

            Order order = CreateOrder(customer);
            order.OrderDate = DateTime.Today;

            HappyHoursCanDiscountStrategy canDiscountStrategy = new HappyHoursCanDiscountStrategy(TimeSpan.Parse("8:30"), TimeSpan.Parse("15:00"));
            PercentageGetDiscountStrategy getDiscountStrategy = new PercentageGetDiscountStrategy(0.1m);
           
            string json = JsonSerializer.Serialize(getDiscountStrategy);

            // var getDiscountStrategy2 = JsonSerializer.Deserialize<PercentageGetDiscountStrategy>(json);

            SmartOrderCalculator orderCalculator = new SmartOrderCalculator(canDiscountStrategy, getDiscountStrategy);

            var discount = orderCalculator.CalculateDiscount(order);


            Console.WriteLine($"Original amount: {order.Amount:C2} Discount: {discount:C2}");


        }

        private static void HappyHoursOrderCalculatorTest()
        {
            Customer customer = new Customer("Anna", "Kowalska");

            Order order = CreateOrder(customer);
            order.OrderDate = DateTime.Today;

            IDiscountStrategy discountStrategy = new HolidayFixedDiscountStrategy(DateTime.Today, 10);

            OrderCalculator calculator = new OrderCalculator(discountStrategy);
            decimal discount = calculator.CalculateDiscount(order);

            Console.WriteLine($"Original amount: {order.Amount:C2} Discount: {discount:C2}");
        }

        private static void GenderOrderCalculatorTest()
        {
            Customer customer = new Customer("Anna", "Kowalska");

            Order order = CreateOrder(customer);

            GenderOrderCalculator calculator = new GenderOrderCalculator();
            decimal discount = calculator.CalculateDiscount(order);

            Console.WriteLine($"Original amount: {order.Amount:C2} Discount: {discount:C2}");
        }

        private static Order CreateOrder(Customer customer)
        {
            Product product1 = new Product(1, "Książka C#", unitPrice: 100m);
            Product product2 = new Product(2, "Książka Praktyczne Wzorce projektowe w C#", unitPrice: 150m);
            Product product3 = new Product(3, "Zakładka do książki", unitPrice: 10m);

            Order order = new Order(DateTime.Parse("2020-06-12 14:59"), customer);
            order.AddDetail(product1);
            order.AddDetail(product2);
            order.AddDetail(product3, 5);

            return order;
        }
    }


    #region Models

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
}
