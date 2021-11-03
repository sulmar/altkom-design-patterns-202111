using System;
using System.Collections.Generic;
using System.Linq;

namespace PrototypePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Prototype Pattern!");
            InvoiceCopyTest();

            // ReservationTest();
        }

        private static void ReservationTest()
        {
            Room room = new Room(0, "010", 3);

            Person person = new Person("John", "Smith");

            Reservation reservation = new Reservation(room, DateTime.Today, DateTime.Today.AddDays(4), person);
            reservation.Breakfast = new Food(FoodType.Continental);

            Display(reservation);

            Reservation secondReservation = new Reservation(reservation.Room,
                DateTime.Today.AddMonths(1),
                DateTime.Today.AddMonths(1).AddDays(7),
                person);

            secondReservation.Breakfast = new Food(FoodType.Regional);

            Display(secondReservation);


        }

        private static void Display(Reservation reservation)
        {
            Console.WriteLine($"Booking {reservation.From.ToShortDateString()} - {reservation.To.ToShortDateString()}");
            Console.WriteLine($"for {reservation.Reserving.FullName}");
            Console.WriteLine($"Room nr {reservation.Room.Flat}");
            Console.WriteLine($"Breakfast {reservation.Breakfast.FoodType}");
        }

        private static void InvoiceCopyTest()
        {
            Customer customer = new Customer("John", "Smith");
            Product product1 = new Product("Keyboard", 250);
            Product product2 = new Product("Mouse", 150);

            Invoice invoice = new Invoice("INV 1", DateTime.Parse("2020-06-01"), DateTime.Parse("2020-06-15"), customer);
            invoice.Details.Add(new InvoiceDetail(product1));
            invoice.Details.Add(new InvoiceDetail(product2, 3));

            Console.WriteLine(invoice);

            Invoice invoiceCopy = new Invoice("INV 2", DateTime.Now, DateTime.Now, invoice.Customer);

            Console.WriteLine(invoiceCopy);

            if (ReferenceEquals(invoice, invoiceCopy))
            {
                Console.WriteLine("The same invoice instances");
            }
            else
            {
                Console.WriteLine("Different invoice instances");
            }

            if (ReferenceEquals(invoice.Customer, invoiceCopy.Customer))
            {
                Console.WriteLine("The same customer instances");
            }
            else
            {
                Console.WriteLine("Different customer instances");
            }
        }
    }

    #region Invoice Model

    public class Invoice
    {
        public Invoice(string number, DateTime createDate, DateTime dueDate, Customer customer)
        {
            Number = number;
            CreateDate = createDate;
            DueDate = dueDate;
            Customer = customer;
        }

        public string Number { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DueDate { get; set; }
        public Customer Customer { get; set; }

        public decimal TotalAmount => Details.Sum(d => d.Quantity * d.Amount);

        public ICollection<InvoiceDetail> Details { get; set; }

        public override string ToString()
        {
            return $"Invoice No {Number} {TotalAmount:C2} {Customer.FullName} paid before {DueDate.ToShortDateString()}";
        }
    }

    public class InvoiceDetail
    {
        public InvoiceDetail(Product product, int quantity = 1)
        {
            Product = product;
            Quantity = quantity;
            Amount = product.UnitPrice;
        }

        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"- {Product} {Quantity} {Amount:C2}";
        }
    }

    public class Product
    {
        public Product(string name, decimal unitPrice)
        {
            Name = name;
            UnitPrice = unitPrice;
        }

        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    #endregion

    #region Reservation Model

    public class Room
    {
        public Room(byte floor, string flat,  byte capacity)
        {
            Floor = floor;
            Flat = flat;
            Capacity = capacity;
        }

        public byte Floor { get; set; }
        public byte Capacity { get; set; }
        public string Flat { get; }
    }

    public class Reservation
    {
        public Reservation(Room room, DateTime from, DateTime to, Person reserving)
        {
            Room = room;
            From = from;
            To = to;
            Reserving = reserving;
        }

        public Room Room { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Person Reserving { get; set; }
        public Food Breakfast { get; set; }

    }

    public class Food
    {
        public FoodType FoodType { get; set; }

        public Food(FoodType foodType)
        {
            this.FoodType = foodType;
        }
    }

    public enum FoodType
    {
        Continental,
        Regional,
        Buffet
    }

    public class Person
    {
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    #endregion

}
