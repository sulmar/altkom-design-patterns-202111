using Stateless;
using System;

namespace StatePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello State Pattern!");

            // OrderTest();
            LampTest();

            // CanvasTest();

        }

        private static void CanvasTest()
        {
            Canvas canvas = new Canvas();
            canvas.CurrentTool = new BrushTool();

            canvas.MouseDown();
            canvas.MouseUp();
        }

        private static void LampTest()
        {
            Lamp lamp = new Lamp();

            Console.WriteLine(lamp.Graph);

            Console.WriteLine(lamp.State);

            lamp.PowerOn();
            Console.WriteLine(lamp.State);

            lamp.PowerOn();
            Console.WriteLine(lamp.State);

            lamp.SemiPush();
            Console.WriteLine(lamp.State);

            lamp.SemiPush();
            Console.WriteLine(lamp.State);

            

            lamp.SemiPush();
            Console.WriteLine(lamp.State);
            lamp.SemiPush();
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


    // dotnet add package Stateless
    public class Lamp
    {
        public LampState State => machine.State;

        private StateMachine<LampState, LampTrigger> machine;

        public string Graph => Stateless.Graph.UmlDotGraph.Format(machine.GetInfo());

        private float level = 0.2f;

        public Lamp()
        {
            machine = new StateMachine<LampState, LampTrigger>(LampState.Off);

            machine.Configure(LampState.Off)
                .Permit(LampTrigger.Push, LampState.On);

            machine.Configure(LampState.On)
                .Permit(LampTrigger.Push, LampState.Off)
                .Permit(LampTrigger.SemiPush, LampState.Power30)
                .OnEntry(() => Console.WriteLine("RTG"), "RTG")
                .OnExit(() => Console.WriteLine("bye"), "BYE");

            machine.Configure(LampState.Power30)
                .Permit(LampTrigger.Push, LampState.Off)
                .Permit(LampTrigger.SemiPush, LampState.Power60);

            machine.Configure(LampState.Power60)
                .Permit(LampTrigger.Push, LampState.Off)
                .PermitIf(LampTrigger.SemiPush, LampState.Power90, ()=> level > 0.5f);

            machine.Configure(LampState.Power90)
                .Permit(LampTrigger.Push, LampState.Off);

            machine
                .OnTransitioned(t => Console.WriteLine($"{t.Source} -> {t.Destination}"));

            //    .Permit(LampTrigger.SemiPush, LampState.On);

        }


        public void PowerOn()
        {
            machine.Fire(LampTrigger.Push);
        }

        public void PowerOff()
        {
            machine.Fire(LampTrigger.Push);
        }

        public void SemiPush() => machine.Fire(LampTrigger.SemiPush);

        public bool CanSemiPush => machine.CanFire(LampTrigger.SemiPush);

    }


    public enum LampTrigger
    {
        Push,
        SemiPush
    }

    public enum LampState
    {
        On,
        Off,
        Power30,
        Power60,
        Power90,
    }

    #endregion

}
