namespace TicTacToe
{
    class Move
    {
        public int Position { get; set; }
        public string Letter { get; set; }

        public Move(int position, string letter)
        {
            Position = position;
            Letter = letter;
        }
    }
}