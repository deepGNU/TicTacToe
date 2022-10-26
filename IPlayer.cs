namespace TicTacToe
{
    interface IPlayer
    {
        Board GameBoard { get; set; }
        string Name { get; set; }
        string Letter { get; set; }
        public Move Move();
    }
}