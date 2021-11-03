using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace DecoratorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Decorator Pattern!");

            // CalculateOrderDiscountTest();

            // FileTest();
        }

        private static void CalculateOrderDiscountTest()
        {
            Customer customer = new Customer("Anna", "Kowalska");

            Order order = CreateOrder(customer);

            HappyHoursOrderCalculator calculator1 = new HappyHoursOrderCalculator();
            decimal discount1 = calculator1.CalculateDiscount(order);

            GenderOrderCalculator calculator2 = new GenderOrderCalculator();
            decimal discount2 = calculator2.CalculateDiscount(order);

            decimal discount = discount1 + discount2;

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

        private static void FileTest()
        {
            string path = "lorem-ipsum.txt";
            string output = $"copy of {path}";

            var bytes = File.ReadAllBytes(path);
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (FileStream fs2 = new FileStream(output, FileMode.Create, FileAccess.Write))
            using (GZipStream zipStream = new GZipStream(fs2, CompressionMode.Compress, false))
            {
                zipStream.Write(bytes, 0, bytes.Length);
            }
        }
    }


    #region 

    public abstract class CoffeeDrink
    {
        public abstract string GetDescription();
        public abstract decimal GetCost();

    }

    public class Expresso : CoffeeDrink
    {
        public override decimal GetCost()
        {
            return 5m;
        }

        public override string GetDescription()
        {
            return "Expresso";
        }
    }

    public class Decaffeinated : CoffeeDrink
    {
        public override decimal GetCost()
        {
            return 4m;
        }

        public override string GetDescription()
        {
            return "Decaffeinated";
        }
    }

    public class HeavilyRoastedCoffee : CoffeeDrink
    {
        public override decimal GetCost()
        {
            return 6m;
        }

        public override string GetDescription()
        {
            return "Heavily Roasted";
        }
    }

    #endregion

    #region Salary

    // wynagrodzenie - podstawa 

    // premia

    // dodatek stazowy

    // pod gruszą

    // za trudne warunki

    // nadgodziny

    public abstract class Employee
    {
        public abstract decimal GetSalary();

    }

    public class Junior : Employee
    {
        public override decimal GetSalary()
        {
            return 2000;
        }
    }

    public class Senior : Employee
    {
        public override decimal GetSalary()
        {
            return 5000;
        }
    }

    public class Architect : Employee
    {
        public override decimal GetSalary()
        {
            return 10000;
        }
    }


    public class JuniorBonus : Employee
    {
        public override decimal GetSalary()
        {
            throw new NotImplementedException();
        }
    }

    public class SeniorBonus : Employee
    {
        public override decimal GetSalary()
        {
            throw new NotImplementedException();
        }
    }

    public class ArchitectBonus : Employee
    {
        public override decimal GetSalary()
        {
            throw new NotImplementedException();
        }
    }

    // Dodatek stazowy
    public class JuniorInternshipAllowance : Employee
    {
        public override decimal GetSalary()
        {
            throw new NotImplementedException();
        }
    }

    public class SeniorInternshipAllowance : Employee
    {
        public override decimal GetSalary()
        {
            throw new NotImplementedException();
        }
    }

    public class ArchitectInternshipAllowance : Employee
    {
        public override decimal GetSalary()
        {
            throw new NotImplementedException();
        }
    }

    public class JuniorOvertime : Employee
    {
        public override decimal GetSalary()
        {
            throw new NotImplementedException();
        }
    }

    public class SeniorOvertime
    {

    }

    public class ArchitectOvertime
    {

    }



    #endregion 







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
