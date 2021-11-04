using System;

namespace AdapterPattern
{

    // Concrete Adapter
    public class HyteraRadioAdapter : IRadioAdapter
    {
        // Adaptee
        private HyteraRadio radio;

        public HyteraRadioAdapter()
        {
            radio = new HyteraRadio();
        }

        public void Call(byte channel)
        {
            throw new NotImplementedException();
        }

        public void Send(byte channel, string message)
        {
            radio.Init();
            radio.SendMessage(channel, message);
            radio.Release();
        }
    }

    public class HyteraRadio
    {

        private RadioStatus status;

        public void Init()
        {
            status = RadioStatus.On;
        }

        public void SendMessage(byte channel, string content)
        {
            if (status == RadioStatus.On)
            {
                Console.WriteLine($"CHANNEL {channel}, MESSAGE {content}");
            }
        }

        public void Release()
        {
            status = RadioStatus.Off;
        }

        public enum RadioStatus
        {
            On,
            Off
        }

    }
}
