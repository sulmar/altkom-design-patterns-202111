using System;

namespace StrategyPattern
{
   
    // Abstract Strategy
    public interface IDiscountStrategy : ICanDiscountStrategy, IGetDiscountStrategy
    {

    }

    public interface ICanDiscountStrategy
    {
        bool CanDiscount(Order order);
    }

    public interface IGetDiscountStrategy
    {
        decimal GetDiscount(Order order);
    }

    public class HappyHoursCanDiscountStrategy : ICanDiscountStrategy
    {
        public TimeSpan from { get; private set; }
        public TimeSpan to { get; private set; }
        public HappyHoursCanDiscountStrategy(TimeSpan from, TimeSpan to)
        {
            this.from = from;
            this.to = to;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }
    }

    public class GenderCanDiscountStrategy : ICanDiscountStrategy
    {
        private readonly Gender gender;

        public GenderCanDiscountStrategy(Gender gender)
        {
            this.gender = gender;
        }

        public bool CanDiscount(Order order)
        {
            return order.Customer.Gender == gender;
        }
    }

    public class HolidayCanDiscountStrategy : ICanDiscountStrategy
    {
        private readonly DateTime holiday;

        public HolidayCanDiscountStrategy(DateTime holiday)
        {
            this.holiday = holiday;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate == holiday;
        }
    }

    public class PercentageGetDiscountStrategy : IGetDiscountStrategy
    {
        public  decimal percentage { get; set; }

        public PercentageGetDiscountStrategy(decimal percentage)
        {
            this.percentage = percentage;
        }

        public decimal GetDiscount(Order order)
        {
            return order.Amount * percentage;
        }
    }

    public class FixedGetDiscountStrategy : IGetDiscountStrategy
    {
        private readonly decimal fixedAmount;

        public FixedGetDiscountStrategy(decimal fixedAmount)
        {
            this.fixedAmount = fixedAmount;
        }

        public decimal GetDiscount(Order order)
        {
            return fixedAmount;
        }
    }

    // Concrete Strategy
    public class HappyHoursPercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal percentage;

        public HappyHoursPercentageDiscountStrategy(TimeSpan from, TimeSpan to, decimal percentage)
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

    public class HappyHoursFixedDiscountStrategy : IDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal fixedAmount;

        public HappyHoursFixedDiscountStrategy(TimeSpan from, TimeSpan to, decimal fixedAmount)
        {
            this.from = from;
            this.to = to;
            this.fixedAmount = fixedAmount;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }

        public decimal GetDiscount(Order order)
        {
            return fixedAmount;
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

    public class HolidayFixedDiscountStrategy : IDiscountStrategy
    {
        private readonly DateTime holiday;
        private readonly decimal fixedAmount;

        public HolidayFixedDiscountStrategy(DateTime holiday, decimal fixedAmount)
        {
            this.holiday = holiday;
            this.fixedAmount = fixedAmount;
        }

        public bool CanDiscount(Order order) => order.OrderDate == holiday && order.Amount >= fixedAmount;
        public decimal GetDiscount(Order order) => fixedAmount;
    }

    public class HolidayPercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly DateTime holiday;
        private readonly decimal percentage;

        public HolidayPercentageDiscountStrategy(DateTime holiday, decimal percentage)
        {
            this.holiday = holiday;
            this.percentage = percentage;
        }

        public bool CanDiscount(Order order) => order.OrderDate == holiday;
        public decimal GetDiscount(Order order) => order.Amount * percentage;
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

    public class SmartOrderCalculator
    {
        private readonly ICanDiscountStrategy canDiscountStrategy;
        private readonly IGetDiscountStrategy getDiscountStrategy;

        public SmartOrderCalculator(ICanDiscountStrategy canDiscountStrategy, IGetDiscountStrategy getDiscountStrategy)
        {
            this.canDiscountStrategy = canDiscountStrategy;
            this.getDiscountStrategy = getDiscountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (canDiscountStrategy.CanDiscount(order))
            {
                return getDiscountStrategy.GetDiscount(order);
            }
            else
                return decimal.Zero;
        }
    }
}
