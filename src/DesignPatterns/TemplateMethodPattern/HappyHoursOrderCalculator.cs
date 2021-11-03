namespace TemplateMethodPattern
{
    // Happy Hours - 10% upustu w godzinach od 9 do 15
    public class HappyHoursOrderCalculator
    {
        public decimal CalculateDiscount(Order order)
        {
            if (order.OrderDate.Hour >= 9 && order.OrderDate.Hour <= 15)
            {
                return order.Amount * 0.1m;
            }
            else
                return 0;
        }
    }

}
