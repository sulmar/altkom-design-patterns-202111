using System;

namespace CommandPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Command Pattern!");

            Message message = new Message("555000123", "555888000", "Hello World!");

            if (message.CanPrint())
            {
                message.Print();
            }

            if (message.CanSend())
            {
                message.Send();
            }    
        }
    }

    #region Models

    public class Message
    {
        public Message(string from, string to, string content)
        {
            From = from;
            To = to;
            Content = content;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }

     
        public void Send()
        {
            Console.WriteLine($"Send message from <{From}> to <{To}> {Content}");
        }

        public bool CanSend()
        {
            return !(string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To) || string.IsNullOrEmpty(Content));
        }

        public void Print(byte copies = 1)
        {
            for (int i = 0; i < copies; i++)
            {
                Console.WriteLine($"Print message from <{From}> to <{To}> {Content}");
            }
        }

        public bool CanPrint()
        {
            return string.IsNullOrEmpty(Content);
        }



    }

    #endregion
}
