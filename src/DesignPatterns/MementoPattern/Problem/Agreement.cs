using System;

namespace MementoPattern.Problem
{
    public class Agreement
    {
        public string CourseName { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal TotalAmount { get; set; }

        public Agreement(string courseName, TimeSpan duration, decimal totalAmount)
        {
            CourseName = courseName;
            Duration = duration;
            TotalAmount = totalAmount;
        }

        public override string ToString()
        {
            return $"{CourseName} {Duration.TotalDays} {TotalAmount}";
        }

        public void GiveDiscount(decimal discountAmount)
        {
            TotalAmount -= discountAmount;
        }
    }
}
