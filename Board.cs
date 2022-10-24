using System;

namespace TicTacToe
{
    class Board
    {
        private int boardLength = 3;
        private int boardArea;
        private string[] board;

        public int Area
        {
            get { return boardArea; }
        }

        public Board()
        {
            boardArea = (int)Math.Pow(boardLength, 2);
            board = new string[boardArea];
        }

        public void Init()
        {
            for (int i = 0; i < boardArea; i++)
                board[i] = (i + 1).ToString();
        }

        public Board Clone()
        {
            Board clone = new Board();
            for (int i = 0; i < boardArea; i++)
                clone.board[i] = board[i];
            return clone;
        }

        public void Print()
        {
            Console.Clear();
            for (int row = 0; row < boardLength; row++)
            {
                if (row > 0)
                    System.Console.WriteLine("\n———————————");
                for (int col = 0; col < boardLength; col++)
                {
                    if (col > 0)
                        System.Console.Write("|");
                    System.Console.Write(" " + board[(row * boardLength) + col] + " ");
                }
            }
            System.Console.WriteLine("\n");
        }

        public void ShowMove(Move move)
        {
            MarkMove(move);
            Print();
            Thread.Sleep(200);
            string[] alternate = { " ", move.Letter };
            // Loop must iterate even num of times so move shows in the end.
            for (int i = 0; i < 8; i++)
            {
                Thread.Sleep(100);
                MarkMove(new Move(move.Position, alternate[i % 2]));
                Print();
                System.Console.WriteLine($"Last move: {move.Letter} to {move.Position}.");
            }
        }

        public bool IsLegalMove(int position) =>
            board.Contains(position.ToString());

        public void MarkMove(Move move) =>
            board[move.Position - 1] = move.Letter;

        public bool NoPositions() => !AvailablePositions().Any();

        public List<int> AvailablePositions()
        {
            List<int> positions = new List<int>();

            for (int i = 1; i <= boardArea; i++)
                if (IsLegalMove(i)) positions.Add(i);

            return positions;
        }

        public bool IsWin()
        {
            for (int rowOrCol = 0; rowOrCol < boardLength; rowOrCol++)
            {
                if (LineIsWin(GetRow(rowOrCol)) ||
                    LineIsWin(GetCol(rowOrCol)))
                    return true;
            }

            if (LineIsWin(GetDiagonal1()) ||
                LineIsWin(GetDiagonal2()))
                return true;

            return false;
        }

        private bool LineIsWin(string[] line)
        {
            for (int i = 1; i < boardLength; i++)
                if (line[i] != line[i - 1]) return false;

            return true;
        }

        private string[] GetRow(int rowNum)
        {
            return Enumerable.Range(0, boardLength)
                    .Select(x => board[x + (rowNum * boardLength)])
                    .ToArray();
        }

        private string[] GetCol(int colNum)
        {
            return Enumerable.Range(0, boardLength)
                    .Select(x => board[colNum + (x * boardLength)])
                    .ToArray();
        }

        private string[] GetDiagonal1()
        {
            return Enumerable.Range(0, boardLength)
                    .Select(x => board[x + (x * boardLength)])
                    .ToArray();
        }

        private string[] GetDiagonal2()
        {
            return Enumerable.Range(0, boardLength)
                    .Select(x => board[boardLength - 1 - x + (x * boardLength)])
                    .ToArray();
        }
    }
}
