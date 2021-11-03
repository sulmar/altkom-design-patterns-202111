using FactoryMethodPattern.Exercise;
using System;

namespace FactoryMethodPattern
{
    public abstract class PaymentView
    {
        public abstract void Show(Payment payment);
    }

    public class CashPaymentView : PaymentView
    {
        public override void Show(Payment payment)
        {
            Console.WriteLine($"Do zapłaty {payment.TotalAmount}");
            Console.Write("Otrzymano: ");
            decimal.TryParse(Console.ReadLine(), out decimal receivedAmount);

            decimal restAmount = payment.TotalAmount - receivedAmount;

            if (restAmount > 0)
            {
                Console.WriteLine($"Reszta {restAmount}");
            }
        }
    }

    public class CreditCardPaymentView : PaymentView
    {
        public override void Show(Payment payment)
        {
            Console.WriteLine($"Do zapłaty {payment.TotalAmount}");

            Console.WriteLine($"Nawiązywanie połączenia z bankiem...");

            Console.WriteLine("Transakcja zautoryzowana");
        }
    }


    public class BankTransferPaymentView : PaymentView
    {
        public override void Show(Payment payment)
        {
            Console.WriteLine($"Dane do przelewu {payment.TotalAmount}");
        }
    }


    public class PaymentTypeFactory
    {
        public PaymentType Create(string input)
        {
            var paymentType = Enum.Parse<PaymentType>(Console.ReadLine());

            return paymentType;
        }
    }

    public class PaymentViewFactory
    {
        public PaymentView Create(PaymentType paymentType)
        {
            switch(paymentType)
            {
                case PaymentType.Cash: return new CashPaymentView();
                case PaymentType.CreditCard: return new CreditCardPaymentView();
                case PaymentType.BankTransfer: return new BankTransferPaymentView();

                default: throw new NotSupportedException();
            }
        }
    }

    public interface IStringIconFactory
    {
        string Create(PaymentType paymentType);
    }

    public class ConsoleStringIconFactory : IStringIconFactory
    {
        public string Create(PaymentType paymentType)
        {
            return GetIcon(paymentType);
        }

        private static string GetIcon(PaymentType paymentType)
        {
            switch (paymentType)
            {
                case PaymentType.Cash: return "[100]";
                case PaymentType.CreditCard: return "[abc]";
                case PaymentType.BankTransfer: return "[-->]";

                default: return string.Empty;
            }
        }
    }

    public class HtmlStringIconFactory : IStringIconFactory
    {
        public string Create(PaymentType paymentType)
        {
            return GetIcon(paymentType);
        }

        private static string GetIcon(PaymentType paymentType)
        {
            switch (paymentType)
            {
                case PaymentType.Cash: return "<img src='cash.png' alt='Gotówka'></img>";
                case PaymentType.CreditCard: return "<img src='creditcard.png' alt='Karta płatnicza'></img>";
                case PaymentType.BankTransfer: return "<img src='banktransfer.png' alt='Przelew bankowy'></img>";

                default: return string.Empty;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Factory Method Pattern!");

            VisitCalculateAmountTest();

            PaymentTest();
        }



        private static void PaymentTest()
        {
            PaymentTypeFactory paymentTypeFactory = new PaymentTypeFactory();
            PaymentViewFactory paymentViewFactory = new PaymentViewFactory();
            IStringIconFactory stringIconFactory = new HtmlStringIconFactory();

            while (true)
            {
                Console.Write("Podaj kwotę: ");

                decimal.TryParse(Console.ReadLine(), out decimal totalAmount);

                Console.Write("Wybierz rodzaj płatności: (G)otówka (K)karta płatnicza (P)rzelew: ");

                var paymentType = paymentTypeFactory.Create(Console.ReadLine());

                Payment payment = new Payment(paymentType, totalAmount);

                PaymentView paymentView = paymentViewFactory.Create(payment.PaymentType);

                string icon = stringIconFactory.Create(payment.PaymentType);
                Console.WriteLine(icon);                
            }

        }

       

        private static void VisitCalculateAmountTest()
        {
            while (true)
            {
                Console.Write("Podaj rodzaj wizyty: (N)FZ (P)rywatna (F)irma: ");
                string visitType = Console.ReadLine();

                Console.Write("Podaj czas wizyty w minutach: ");
                if (double.TryParse(Console.ReadLine(), out double minutes))
                {
                    TimeSpan duration = TimeSpan.FromMinutes(minutes);

                    Visit visit = new Visit(duration, 100);

                    decimal totalAmount = visit.CalculateCost(visitType);

                    if (totalAmount == 0)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                       if (totalAmount >= 200)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine($"Total amount {totalAmount:C2}");

                    Console.ResetColor();
                }
            }

        }
    }

    #region Models


    public class Visit
    {
        public DateTime VisitDate { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal PricePerHour { get; set; }

        private const decimal companyDiscountPercentage = 0.9m;

        public Visit(TimeSpan duration, decimal pricePerHour)
        {
            VisitDate = DateTime.Now;
            Duration = duration;
            PricePerHour = pricePerHour;
        }

        public decimal CalculateCost(string kind)
        {
            decimal cost = 0;

            if (kind == "N")
            {
                cost = 0;
            }
            else if (kind == "P")
            {
                cost = (decimal)Duration.TotalHours * PricePerHour;
            }
            else if (kind == "F")
            {
                cost = (decimal)Duration.TotalHours * PricePerHour * companyDiscountPercentage;
            }

            return cost;
        }
    }

    #endregion
}
