namespace TicTacToe
{
    class HumanPlayer : Player
    {
        public HumanPlayer(Board board, string letter)
        {
            GameBoard = board;
            Letter = letter;
        }

        public override Move Move()
        {
            System.Console.WriteLine($"It's {Name}'s turn.");
            System.Console.WriteLine($"Please enter a number to place an {Letter} on.");
            int.TryParse(Console.ReadLine(), out int position);

            while (!GameBoard.IsLegalMove(position))
            {
                System.Console.WriteLine("Sorry, that is an illegal move. Please try again.");
                int.TryParse(Console.ReadLine(), out position);
            }
            return new Move(position, Letter);
        }
    }
}