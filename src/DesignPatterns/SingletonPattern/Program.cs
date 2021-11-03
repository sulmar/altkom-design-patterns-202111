using System;
using System.IO;

namespace SingletonPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Singleton Pattern!");

            LoggerTest();

            ApplicationContextTest();
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

    public class Logger
    {
        private string path = "log.txt";

        public void LogInformation(string message)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine($"{DateTime.Now} {message}");
            }
        }
    }

    public class MessageService
    {
        public Logger logger;

        public MessageService()
        {
            logger = new Logger();
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
            logger = new Logger();
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
