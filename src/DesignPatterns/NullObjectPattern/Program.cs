using System;

namespace NullObjectPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Null Object Pattern!");

            IProductRepository productRepository = new FakeProductRepository();

            Product product = productRepository.Get(1);

            // Problem: Zawsze musimy sprawdzać czy obiekt nie jest pusty (null).

            if (product != null)
            {
                product.RateId(3);
            }
        }
    }

    public interface IProductRepository
    {
        Product Get(int id);
    }

    public class FakeProductRepository : IProductRepository
    {
        public Product Get(int id)
        {
            return null;
        }
    }

    public class Product
    {
        private int rate;

        public void RateId(int rate)
        {
            this.rate = rate;
        }

    }
}
