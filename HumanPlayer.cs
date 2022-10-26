namespace TicTacToe
{
    class HumanPlayer : IPlayer
    {
        public Board GameBoard { get; set; }
        public string Name { get; set; }
        public string Letter { get; set; }

        public HumanPlayer(Board board, string letter)
        {
            GameBoard = board;
            Letter = letter;
            Name = "Anon";
        }

        public HumanPlayer(Board board, string letter, string name)
                    : this(board, letter) { Name = name; }

        public Move Move()
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