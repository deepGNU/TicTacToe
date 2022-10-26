using System;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Game game = new Game();
                game.Run();
            } while (Game.ReadChoice("\nWould you like to play another game?",
                                     "Play another game",
                                     "Quit") == 1);

            System.Console.WriteLine("Thanks for playing!");
        }
    }
}