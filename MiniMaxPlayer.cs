namespace TicTacToe
{
    class MiniMaxPlayer : IPlayer
    {
        private int movePosition;
        private const int INITIAL_DEPTH = 0;
        private const int POINTS = 10;
        private const string X = "X";
        private const string O = "O";
        private string otherLetter() => Letter == X ? O : X;
        public Board GameBoard { get; set; }
        public string Name { get; set; }
        public string Letter { get; set; }

        public MiniMaxPlayer(Board board, string letter)
        {
            GameBoard = board;
            Letter = letter;
            Name = "Computer";
        }

        public Move Move()
        {
            System.Console.WriteLine("Computer is thinking...");
            MiniMax(GameBoard.Clone());
            return new Move(movePosition, Letter);
        }

        // Recursive algorithm to find best move. 
        private int MiniMax(Board board, int depth = INITIAL_DEPTH,
                            bool isMyTurn = true)
        {
            bool isWin = board.IsWin();
            // If game is over, returning score. (Flipping turn because
            // if there is a winner, it's the previous player.)
            if (isWin || board.NoPositions())
                return Score(isWin, !isMyTurn, depth);

            string letter;
            if (isMyTurn) letter = Letter;
            else letter = otherLetter();
            List<int> positions = board.AvailablePositions();
            List<int> scores = new List<int>();

            // Adding each possible move to a new board and adding
            // new board's score to list of scores.
            foreach (int position in positions)
            {
                Board futureBoard = board.Clone();
                futureBoard.MarkMove(new Move(position, letter));
                scores.Add(MiniMax(futureBoard, depth + 1, !isMyTurn));
            }

            // We assume perfect play; therefore, if it's my turn, the total
            // score of the current state is equal to the score of my best
            // possible move; whereas, if it's my opponent's turn, the score
            // is equal to my opponent's best move, which is worst for me.
            // Returning the maximum or minumum score accordingly.
            if (isMyTurn)
            {
                int maxScore = scores.Max();
                // Setting my move to best move.
                movePosition = positions[scores.IndexOf(maxScore)];
                return maxScore;
            };

            return scores.Min();
        }

        // Returns a number of points minus the depth (num of moves ahead)
        // for a win, a negative number of points plus the depth for a loss,
        // and zero points for a draw.
        private int Score(bool isWin, bool wasMyTurn, int depth)
        {
            if (isWin)
            {
                if (wasMyTurn) return POINTS - depth;
                return depth - POINTS;
            }
            return 0;
        }
    }
}