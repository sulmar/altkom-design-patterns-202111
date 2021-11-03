using System;

namespace StatePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello State Pattern!");

            // OrderTest();

            Lamp lamp = new Lamp();
            Console.WriteLine(lamp.State);

            lamp.PowerOn();
            Console.WriteLine(lamp.State);

            lamp.PowerOff();
            Console.WriteLine(lamp.State);

            lamp.PowerOff();
            Console.WriteLine(lamp.State);


        }

        private static void OrderTest()
        {
            Order order = Order.Create();

            order.Completion();

            if (order.Status == OrderStatus.Completion)
            {
                order.Status = OrderStatus.Sent;
                Console.WriteLine("Your order was sent.");
            }

            order.Cancel();
        }
    }

    #region Models

    public class Order
    {
        public Order(string orderNumber)
        {
            Status = OrderStatus.Created;

            OrderNumber = orderNumber;
            OrderDate = DateTime.Now;
         
        }

        public DateTime OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public OrderStatus Status { get; set; }

        private static int indexer;

        public static Order Create()
        {
            Order order = new Order($"Order #{indexer++}");

            if (order.Status == OrderStatus.Created)
            {
                Console.WriteLine("Thank you for your order");
            }

            return order;
        }

        public void Completion()
        {
            if (Status == OrderStatus.Created)
            {
                this.Status = OrderStatus.Completion;

                Console.WriteLine("Your order is in progress");
            }
        }

        public void Cancel()
        {
            if (this.Status == OrderStatus.Created || this.Status == OrderStatus.Completion)
            {
                this.Status = OrderStatus.Canceled;

                Console.WriteLine("Your order was cancelled.");
            }
        }

    }

    public enum OrderStatus
    {
        Created,
        Completion,
        Sent,
        Canceled,
        Done
    }

    public class Lamp
    {
        public LampState State { get; set; }

        public Lamp()
        {
            State = LampState.Off;
        }

        public void PowerOn()
        {
            if (State == LampState.Off)
            {
                State = LampState.On;
            }
            else
                throw new InvalidOperationException($"state {State}");

        }

        public void PowerOff()
        {
            if (State == LampState.On)
            {
                State = LampState.Off;
            }
            else
                throw new InvalidOperationException($"state {State}");

        }



    }

    public enum LampState
    {
        On,
        Off
    }

    #endregion

}
