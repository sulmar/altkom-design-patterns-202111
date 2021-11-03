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
            while (true)
            {
                Console.Write("Podaj kwotę: ");

                decimal.TryParse(Console.ReadLine(), out decimal totalAmount);

                Console.Write("Wybierz rodzaj płatności: (G)otówka (K)karta płatnicza (P)rzelew: ");

                var paymentType = Enum.Parse<PaymentType>(Console.ReadLine());

                Payment payment = new Payment(paymentType, totalAmount);

                if (payment.PaymentType == PaymentType.Cash)
                {
                    CashPaymentView cashPaymentView = new CashPaymentView();
                    cashPaymentView.Show(payment);
                }
                else
                if (payment.PaymentType == PaymentType.CreditCard)
                {
                    CreditCardPaymentView creditCardView = new CreditCardPaymentView();
                    creditCardView.Show(payment);
                }
                else
                if (payment.PaymentType == PaymentType.BankTransfer)
                {
                    BankTransferPaymentView bankTransferPaymentView = new BankTransferPaymentView();
                    bankTransferPaymentView.Show(payment);
                }

                string icon = GetIcon(payment);
                Console.WriteLine(icon);                
            }

        }

        private static string GetIcon(Payment payment)
        {
            switch (payment.PaymentType)
            {
                case PaymentType.Cash: return "[100]"; 
                case PaymentType.CreditCard: return "[abc]"; 
                case PaymentType.BankTransfer: return "[-->]";

                default: return string.Empty;
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
