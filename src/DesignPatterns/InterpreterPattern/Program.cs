using System;
using System.Collections.Generic;
using System.Linq;

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

            var parser = new Parser(new ExpressionFactory());

            int result = parser.Evaluate(expression);

            Console.WriteLine($"{expression} = {result}");

        }
    }

    #region Model

    public class Parser
    {
        private IList<IExpression> parseTree = new List<IExpression>();

        private IExpressionFactory expressionFactory;

        public Parser(IExpressionFactory expressionFactory)
        {
            this.expressionFactory = expressionFactory;
        }

        private void Parse(string s)
        {
            var tokens = s.Split(' ');

            BuildParseTree(tokens);          
        }

        private void BuildParseTree(string[] tokens)
        {
            foreach (var token in tokens)
            {
                parseTree.Add(expressionFactory.Create(token));
            }
        }

        public int Evaluate(string s)
        {
            Parse(s);


            // Context
            var context = new Stack<int>();

            foreach (var expression in parseTree)
            {
                expression.Interpret(context);
            }

            return context.Pop();
        }
    }

    // Abstract Expression
    public interface IExpression
    {
        void Interpret(Stack<int> s);
    }

    // TerminalExpression 
    public class NumberExpression : IExpression
    {
        private readonly int number;

        public NumberExpression(int number)
        {
            this.number = number;
        }

        public void Interpret(Stack<int> s)
        {
            s.Push(number);
        }
    }

    // TerminalExpression 
    public class PlusExpression : IExpression
    {
        public void Interpret(Stack<int> s)
        {
            s.Push(s.Pop() + s.Pop());
        }
    }

    // TerminalExpression 
    public class MinusExpression : IExpression
    {
        public void Interpret(Stack<int> s)
        {
            s.Push(-s.Pop() + s.Pop());
        }
    }

    // TerminalExpression
    public class MultiplyExpression : IExpression
    {
        public void Interpret(Stack<int> s)
        {
            s.Push(s.Pop() * s.Pop());
        }
    }


    public interface IExpressionFactory
    {
        public IExpression Create(string token);
    }

    public class ExpressionFactory : IExpressionFactory
    {
        public IExpression Create(string token) => token switch
        {
            "+" => new PlusExpression(),
            "-" => new MinusExpression(),
            "*" => new MultiplyExpression(),
            _ => new NumberExpression(int.Parse(token)),
        };
    }

    #endregion
}
