using System;

namespace AdapterPattern
{
    // Abstract Adapter
    public interface IRadioAdapter
    {
        void Send(byte channel, string message);
        void Call(byte channel);
    }

    // Concrete Adapter
    public class MotorolaRadioAdapter : IRadioAdapter
    {
        private readonly string pincode;

        // Adaptee
        private MotorolaRadio radio;

        public MotorolaRadioAdapter(string pincode)
        {
            radio = new MotorolaRadio();

            this.pincode = pincode;
        }

        public void Call(byte channel)
        {
            throw new NotImplementedException();
        }

        public void Send(byte channel, string message)
        {
            radio.PowerOn(pincode);
            radio.SelectChannel(channel);
            radio.Send(message);
            radio.PowerOff();
        }
    }

    public class MotorolaRadio
    {
        private bool enabled;

        private byte? selectedChannel;

        public MotorolaRadio()
        {
            enabled = false;
        }

        public void PowerOn(string pincode)
        {
            if (pincode == "1234")
            {
                enabled = true;
            }
        }

        public void SelectChannel(byte channel)
        {
            this.selectedChannel = channel;
        }

        public void Send(string message)
        {
            if (enabled && selectedChannel!=null)
            {
                Console.WriteLine($"<Xml><Send Channel={selectedChannel}><Message>{message}</Message></xml>");
            }
        }

        public void PowerOff()
        {
            enabled = false;
        }



    }


    public class PanasonicRadioAdapter : IRadioAdapter
    {
        public void Call(byte channel)
        {
            throw new NotImplementedException();
        }

        public void Send(byte channel, string message)
        {
            throw new NotImplementedException();
        }
    }
}
