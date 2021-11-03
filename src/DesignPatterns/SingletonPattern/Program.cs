﻿using System;
using System.IO;
using System.Threading;

namespace SingletonPattern
{
    public struct Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Printer
    {
        public Printer()
        {
            Console.WriteLine("Initializing...");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Console.WriteLine("Ready.");
        }

        public void Print(string content)
        {
            Console.WriteLine($"Printing {content}");
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Singleton Pattern!");

            //LazyTest();

            //LoggerTest();

            PrinterTest();

            ApplicationContextTest();
        }

        private static void PrinterTest()
        {
            Printer printer1 = PrinterSingleton.Instance;

            Printer printer2 = PrinterSingleton.Instance;

            if (ReferenceEquals(printer1, printer2))
            {
                Console.WriteLine("The same instances.");
            }
        }

        private static void LazyTest()
        {
            Lazy<Printer> lazyPrinter = new Lazy<Printer>(()=>new Printer());

            // ...
            Printer printer = lazyPrinter.Value;

            printer.Print("Hello World!");

        }

        private static void LoggerTest()
        {
            MessageService messageService = new MessageService();
            PrintService printService = new PrintService();

            messageService.Send("Hello World!");
            printService.Print("Hello World!", 3);

            if (ReferenceEquals(messageService.logger, printService.logger))
            {
                Console.WriteLine("The same instances");
            }
            else
            {
                Console.WriteLine("Different instances");
            }
        }

        private static void ApplicationContextTest()
        {
            ApplicationContext context = new ApplicationContext();
            context.LoggedDate = DateTime.Now;
            context.LoggedUser = "user1";

            Module1 module1 = new Module1();
            Module2 module2 = new Module2();


            module1.CustomerChanged();
            module2.ShowSelectedCustomer();


        }
    }

    #region Logger

    public class ColorLogger : Logger
    {
        protected ColorLogger()
            : base()
        {

        }
    }

    

    public class Logger
    {
        private string path = "log.txt";

        protected Logger()
        {                
        }

        private static Lazy<Logger> lazyLogger = new Lazy<Logger>(() => new Logger());

        public static Logger Instance => lazyLogger.Value;

        public void LogInformation(string message)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine($"{DateTime.Now} {message}");
            }
        }
    }

    public class LocationSingleton : Singleton<Location>
    {

    }

    public class PrinterSingleton : Singleton<Printer>
    {
        protected PrinterSingleton()
        {

        }
    }

    public class Singleton<T>
        where T : new() // constraint
    {
        protected Singleton()
        {

        }

        private static Lazy<T> lazyLogger = new Lazy<T>(() => new T());

        public static T Instance => lazyLogger.Value;
    }

    public class MessageService
    {
        public Logger logger;

        public MessageService()
        {
            logger = ColorLogger.Instance;
        }

        public void Send(string message)
        {
            logger.LogInformation($"Send {message}");
        }
    }

    public class PrintService
    {
        public Logger logger;

        public PrintService()
        {
            logger = Logger.Instance;
        }

        public void Print(string content, int copies)
        {
            for (int i = 1; i < copies+1; i++)
            {
                logger.LogInformation($"Print {i} copy of {content}");
            }
        }




    }


    #endregion


    #region ApplicationContext

    public class ApplicationContext
    {
        public string LoggedUser { get; set; }
        public DateTime LoggedDate { get; set; }
        public Customer SelectedCustomer { get; set; }

        public Location CurrentLocation { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Module1
    {
        private ApplicationContext applicationContext;

        public Module1()
        {
            applicationContext = new ApplicationContext();
        }

        public void CustomerChanged()
        {
            applicationContext.SelectedCustomer = new Customer { Id = 1, Name = "Customer 1" };
        }
    }

    public class Module2
    {
        private ApplicationContext applicationContext;

        public Module2()
        {
            applicationContext = new ApplicationContext();
        }

        public void ShowSelectedCustomer()
        {
            Console.WriteLine(applicationContext.SelectedCustomer?.Name);
        }
    }

    #endregion
}
