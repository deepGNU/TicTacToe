namespace TicTacToe
{
    class RandomPlayer : IPlayer
    {
        public Board GameBoard { get; set; }
        public string Name { get; set; }
        public string Letter { get; set; }
        public RandomPlayer(Board board, string letter)
        {
            GameBoard = board;
            Letter = letter;
            Name = "Computer";
        }

        public Move Move()
        {
            List<int> positions = GameBoard.AvailablePositions();
            int randomIndex = new Random().Next(0, positions.Count());
            return new Move(positions[randomIndex], Letter);
        }
    }
}