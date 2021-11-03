namespace DecoratorPattern
{
    // Gender - 20% upustu dla kobiet
    public class GenderOrderCalculator
    {
        public decimal CalculateDiscount(Order order)
        {
            if (order.Customer.Gender == Gender.Female)
            {
                return order.Amount * 0.2m;
            }
            else
                return 0;
        }
    }

}
