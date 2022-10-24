namespace TicTacToe
{
    class RandomPlayer : Player
    {
        public RandomPlayer(Board board, string letter)
        {
            GameBoard = board;
            Name = "Computer";
            Letter = letter;
        }

        public override Move Move()
        {
            List<int> positions = GameBoard.AvailablePositions();
            int randomIndex = new Random().Next(0, positions.Count());
            return new Move(positions[randomIndex], Letter);
        }
    }
}