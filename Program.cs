using System;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            int playAgain = 1;

            do
            {
                Game game = new Game();
                game.Run();
                System.Console.WriteLine("\nWould you like to play another game?");
                System.Console.WriteLine("1. Play another game.");
                System.Console.WriteLine("2. Quit.");
            } while (Game.GetChoice() == playAgain);

            System.Console.WriteLine("Thanks for playing!");
        }
    }
}