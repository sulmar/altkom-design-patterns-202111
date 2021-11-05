using System;

namespace NullObjectPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Null Object Pattern!");

            IProductRepository productRepository = new FakeProductRepository();

            ProductBase product = productRepository.Get(1);

            // Problem: Zawsze musimy sprawdzać czy obiekt nie jest pusty (null).

            product.RateId(3);

            
        }
    }

    public interface IProductRepository
    {
        ProductBase Get(int id);
    }

    public class FakeProductRepository : IProductRepository
    {
        public ProductBase Get(int id)
        {
            if (id == 2)
            {
                return new Product();
            }

            return new NullProduct();
        }
    }
    

    // Abstract Object
    public abstract class ProductBase
    {
        protected int rate;

        public string Name { get; set; }

        public abstract void RateId(int rate);
    }

    // Real Object
    public class Product : ProductBase
    {
        public override void RateId(int rate)
        {
            this.rate = rate;
        }
    }

    // Null Object
    public class NullProduct : ProductBase
    {
        public NullProduct()
        {
            Name = "Bez nazwy";
        }

        public override void RateId(int rate)
        {
            // nic nie rób
        }
    }
}
