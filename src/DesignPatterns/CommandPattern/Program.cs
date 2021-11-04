using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CommandPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Command Pattern!");

            Message message = new Message("555000123", "555888000", "Hello World!");

            ConcurrentQueue<ICommand> queue = new ConcurrentQueue<ICommand>();

            queue.Enqueue(new PrintCommand(message, 10));
            queue.Enqueue(new SendCommand(message));
            queue.Enqueue(new SendCommand(message));
            queue.Enqueue(new PrintCommand(message, 5));
            queue.Enqueue(new SendCommand(message));

            while (queue.Any())
            {
                // ICommand command = commands.Dequeue();

                if (queue.TryDequeue(out ICommand command))
                {
                    try
                    {
                        command.Execute();
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }

        }
    }

    #region Models

    // Abstract Command
    public interface ICommand
    {
        void Execute();

        bool CanExecute();
    }


    // Concrete Command
    public class SendCommand : ICommand
    {
        public Message Message { get; }

        public SendCommand(Message message)
        {
            this.Message = message;
        }

        public void Execute()
        {
            if (CanExecute())
            {
                Console.WriteLine($"Send message from <{Message.From}> to <{Message.To}> {Message.Content}");
            }
            else
                throw new InvalidOperationException("From, To, Content is requeried.");
        }

        public bool CanExecute()
        {
            return !(string.IsNullOrEmpty(Message.From) || string.IsNullOrEmpty(Message.To) || string.IsNullOrEmpty(Message.Content));
        }
    }

    // Concrete Command
    public class PrintCommand : ICommand
    {
        private int papers = 5;
        private byte copies;

        public Message Message { get; }

        public PrintCommand(Message message, byte copies = 1)
        {
            this.Message = message;
            this.copies = copies;
        }

        public void Execute()
        {
            if (CanExecute())
            {
                for (int i = 0; i < copies; i++)
                {

                    Console.WriteLine($"Print message from <{Message.From}> to <{Message.To}> {Message.Content}");
                    --papers;
                }
            }
            else
                throw new InvalidOperationException("No paper");
        }

        public bool CanExecute()
        {
            return !string.IsNullOrEmpty(Message.Content) && copies <= papers;
        }
    }

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
    }

    #endregion
}
