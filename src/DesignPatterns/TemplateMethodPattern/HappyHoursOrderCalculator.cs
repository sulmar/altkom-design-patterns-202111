using System;

namespace TemplateMethodPattern
{
    public class Shape
    {

    }

    public abstract class ShapeCalculator
    {
        public abstract bool Check1(Shape shape);

        public abstract bool Check2(Shape shape);

        public abstract decimal Calculate(Shape shape);

        public decimal CalculateArea(Shape shape)
        {
            // 1. Check 1
            if (Check1(shape))
            {

                // 2. Check 2
                if (Check2(shape))
                {
                    return Calculate(shape);
                }
            }

            return decimal.Zero;
        }
    }


    public interface IOrderCalculator
    {
        decimal CalculateDiscount(Order order);
    }



    // Template Method
    public abstract class OrderCalculator : IOrderCalculator
    {
        public abstract bool CanDiscount(Order order);
        public abstract decimal GetDiscount(Order order);

        public decimal CalculateDiscount(Order order)   // Template Method
        {
            if (CanDiscount(order))    // Predykat
            {
                return GetDiscount(order);     // Upust
            }
            else
                return decimal.Zero;
        }
    }

    // Happy Hours - 10% upustu w godzinach od 9 do 15
    public class HappyHoursOrderCalculator : OrderCalculator, IOrderCalculator
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal percentage;

        public HappyHoursOrderCalculator(TimeSpan from, TimeSpan to, decimal percentage)
        {
            this.from = from;
            this.to = to;
            this.percentage = percentage;
        }

        public override bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

        public override decimal GetDiscount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class GenderOrderCalculator : OrderCalculator, IOrderCalculator
    {
        private readonly Gender gender;
        private readonly decimal percentage;

        public GenderOrderCalculator(Gender gender, decimal percentage)
        {
            this.gender = gender;
            this.percentage = percentage;
        }

        public override bool CanDiscount(Order order)
        {
            return order.Customer.Gender == gender;
        }

        public override decimal GetDiscount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class HolidayOrderCalculator : OrderCalculator, IOrderCalculator
    {
        private readonly DateTime holiday;
        private readonly decimal fixedDiscount;

        public HolidayOrderCalculator(DateTime holiday, decimal fixedDiscount)
        {
            this.holiday = holiday;
            this.fixedDiscount = fixedDiscount;
        }

        public override bool CanDiscount(Order order)
        {
            return order.OrderDate == holiday && order.Amount >= fixedDiscount;
        }

        public override decimal GetDiscount(Order order)
        {
            return fixedDiscount;
        }
    }

}
