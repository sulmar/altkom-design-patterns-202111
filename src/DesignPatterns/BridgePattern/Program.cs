using System;

namespace BridgePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bridge Pattern!");

            RemoteControlTest();
        }

        private static void RemoteControlTest()
        {
            RemoteControl remoteControl = new SonyRemoteControl();

            remoteControl.TurnOn();
            remoteControl.TurnOff();

            AdvancedRemoteControl advancedRemoteControl = new SonyAdvancedRemoteControl();

            advancedRemoteControl.SetChannel(100);
        }
    }


    // Basic Remote Control (TurnOn, TurnOff)
    // Advanced Remote Control (SetChannel)
    // Movie Remote Control (Play, Pause, Rewind)
    public abstract class RemoteControl
    {
        public abstract void TurnOn();

        public abstract void TurnOff();
    }

    public abstract class AdvancedRemoteControl : RemoteControl
    {
        public abstract void SetChannel(byte number);
    }

    public class SonyRemoteControl : RemoteControl
    {
        public override void TurnOff()
        {
            Console.WriteLine("Sony: TurnOff");
        }

        public override void TurnOn()
        {
            Console.WriteLine("Sony: TurnOn");
        }
    }

    public class SamsungRemoteControl : RemoteControl
    {
        public override void TurnOff()
        {
            Console.WriteLine("Samsung: TurnOff");
        }

        public override void TurnOn()
        {
            Console.WriteLine("Samsung: TurnOn");
        }
    }

    public class SonyAdvancedRemoteControl : AdvancedRemoteControl
    {
        public override void SetChannel(byte number)
        {
            Console.WriteLine($"Sony: Set Channel {number}");
        }

        public override void TurnOff()
        {
            Console.WriteLine("Sony: TurnOff");
        }

        public override void TurnOn()
        {
            Console.WriteLine("Sony: TurnOn");
        }
    }

    public class SamsungAdvancedRemoteControl : AdvancedRemoteControl
    {
        public override void SetChannel(byte number)
        {
            Console.WriteLine($"Samsung: Set Channel {number}");
        }

        public override void TurnOff()
        {
            Console.WriteLine("Samsung: TurnOff");
        }

        public override void TurnOn()
        {
            Console.WriteLine("Samsung: TurnOn");
        }
    }
}
