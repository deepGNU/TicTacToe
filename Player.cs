namespace TicTacToe
{
    class Player
    {
        public Board? GameBoard { get; set; }
        public string? Name { get; set; }
        public string? Letter { get; set; }
        public virtual Move Move() => null;
    }
}