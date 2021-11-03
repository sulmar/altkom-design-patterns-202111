using System;

namespace InterpreterPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Interpreter Pattern!");

            ParserTest();
        }

        private static void ParserTest()
        {
            // (2 + 3) * (1 - 2)
            string expression = "2 3 + 1 - 2 *";

            var parser = new Parser();

            int result = parser.Evaluate(expression);

            Console.WriteLine($"{expression} = {result}");

        }
    }

    #region Model

    public class Parser
    {
        public int Evaluate(string s)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
