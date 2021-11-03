using System;
using System.Collections.Generic;
using System.Text;

namespace FactoryMethodPattern.Exercise
{
    public class Payment
    {
        public DateTime PaymentDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal TotalAmount { get; set; }

        public Payment(DateTime paymentDate, PaymentType paymentType, decimal totalAmount)
        {
            PaymentDate = paymentDate;
            PaymentType = paymentType;
            TotalAmount = totalAmount;
        }

        public Payment(PaymentType paymentType, decimal totalAmount)
            :this(DateTime.Now, paymentType, totalAmount)
        { }

    }

    public enum PaymentType
    {
        Cash,
        CreditCard,
        BankTransfer
    }
}
