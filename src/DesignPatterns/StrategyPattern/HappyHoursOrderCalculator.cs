using System;

namespace StrategyPattern
{
   
    // Abstract Strategy
    public interface IDiscountStrategy
    {
        bool CanDiscount(Order order);
        decimal GetDiscount(Order order);
    }

    // Concrete Strategy
    public class HappyHoursDiscountStrategy : IDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal percentage;

        public HappyHoursDiscountStrategy(TimeSpan from, TimeSpan to, decimal percentage)
        {
            this.from = from;
            this.to = to;
            this.percentage = percentage;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

        public decimal GetDiscount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class GenderDiscountStrategy : IDiscountStrategy
    {
        private readonly Gender gender;
        private readonly decimal percentage;

        public GenderDiscountStrategy(Gender gender, decimal percentage)
        {
            this.gender = gender;
            this.percentage = percentage;
        }

        public bool CanDiscount(Order order)
        {
            return order.Customer.Gender == gender;
        }

        public decimal GetDiscount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class HolidayDiscountStrategy : IDiscountStrategy
    {
        private readonly DateTime holiday;
        private readonly decimal fixedAmount;

        public HolidayDiscountStrategy(DateTime holiday, decimal fixedAmount)
        {
            this.holiday = holiday;
            this.fixedAmount = fixedAmount;
        }

        public bool CanDiscount(Order order) => order.OrderDate == holiday && order.Amount >= fixedAmount;
        public decimal GetDiscount(Order order) => fixedAmount;
    }

    public class OrderCalculator
    {
        private readonly IDiscountStrategy discountStrategy;

        public OrderCalculator(IDiscountStrategy discountStrategy)
        {
            this.discountStrategy = discountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (discountStrategy.CanDiscount(order))
            {
                return discountStrategy.GetDiscount(order);
            }
            else
                return decimal.Zero;
        }
    }
}
