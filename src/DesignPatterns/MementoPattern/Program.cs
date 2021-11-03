using MementoPattern.Exercise;
using MementoPattern.Problem;
using System;

namespace MementoPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Memento Pattern!");

            ArticleTest();

            AgreementTest();
        }

        private static void ArticleTest()
        {
            var article = new Article();
            article.Content = "a";
            article.Content = "b";
            article.Content = "c";
            
            // TODO: Undo
        }

        private static void AgreementTest()
        {
            Agreement agreement = new Agreement("Design Patterns", TimeSpan.FromDays(3), 1000);
            // TODO: Save version #1            
            
            agreement.GiveDiscount(100);
            // TODO: Save version #2            

            // Save
            agreement.Duration = TimeSpan.FromDays(4);
            agreement.GiveDiscount(50);
            // TODO: Save version #3

            // TODO: Back to version #2

            // TODO: Back to version #1

            // TODO: Back to original

        }

        private static void PlayerTest()
        {
            Player player = new Player();

            Console.WriteLine(player);

            player.AddPoints(10);
            player.Hit();
            Console.WriteLine(player);

            // TODO: Save checkpoint
            
            player.AddPoints(30);
            player.Hit();
            player.AddPoints(20);
            player.Hit();
            player.UpLevel();
            Console.WriteLine(player);

            // TODO: Rollback do checkpoint
            
            Console.WriteLine(player);

        }


    }
}
