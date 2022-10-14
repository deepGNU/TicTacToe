using System;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }

    class Game
    {
        Board board;
        Player currPlayer;
        Player[] players;
        Move move;
        bool isWin = false;

        public Game()
        {
            board = new Board();
            board.Init();
            SetPlayers();
        }

        public void Run()
        {
            board.Print();

            for (int i = 0; !isWin && i < board.Area; i++)
            {
                SetPlayer(i);
                move = currPlayer.Move();
                board.MarkMove(move);
                board.Print();
                board.BlinkMove(move);
                isWin = board.IsWin();
            }

            System.Console.WriteLine("Game over.");
            if (isWin)
                System.Console.WriteLine($"{currPlayer.Name} wins!");
            else
                System.Console.WriteLine("The game is drawn.");
        }

        private void SetPlayer(int turn) =>
            currPlayer = players[turn % 2];

        private void SetPlayers()
        {
            string x = "X";
            string o = "O";
            Player player1 = new HumanPlayer(board, x);
            Player player2 = new HumanPlayer(board, o);

            System.Console.WriteLine("Would you like to play a person, or the computer?");
            System.Console.WriteLine("1. Play a person.");
            System.Console.WriteLine("2. Play the computer.");

            switch (GetChoice())
            {
                case 1:
                    System.Console.WriteLine("Please enter the name of player 1:");
                    player1.Name = Console.ReadLine();
                    System.Console.WriteLine("Please enter the name of player 2:");
                    player2.Name = Console.ReadLine();
                    Console.Clear();
                    break;
                case 2:
                    System.Console.WriteLine("Please select a level:");
                    System.Console.WriteLine("1. Easy (moves randomly).");
                    System.Console.WriteLine("2. Hard (uses minimax algorithm to play to at least a draw).");

                    switch (GetChoice())
                    {
                        case 1:
                            player2 = new RandomPlayer(board, o);
                            break;
                        case 2:
                            player2 = new MiniMaxPlayer(board, o);
                            break;
                    }

                    System.Console.WriteLine("Please enter your name:");
                    player1.Name = Console.ReadLine();
                    Console.Clear();
                    break;
            }

            System.Console.WriteLine($"{player1.Name}, would you like to be '{x}', or '{o}'? ({x} goes first.)");
            System.Console.WriteLine($"1. {x}");
            System.Console.WriteLine($"2. {o}");

            switch (GetChoice())
            {
                case 1:
                    players = new Player[] { player1, player2 };
                    break;
                case 2:
                    (player1.Letter, player2.Letter) =
                    (player2.Letter, player1.Letter);
                    players = new Player[] { player2, player1 };
                    break;
            }
        }

        static int GetChoice()
        {
            int[] validChoices = { 1, 2 };
            int.TryParse(Console.ReadLine(), out int choice);
            while (!validChoices.Contains(choice))
            {
                System.Console.WriteLine("Sorry, that is an invalid choice. Please try again.");
                int.TryParse(Console.ReadLine(), out choice);
            }
            Console.Clear();
            return choice;
        }

        // private void BlinkMove(Move move)
        // {
        //     Thread.Sleep(200);
        //     string[] alternate = { " ", move.Letter };
        //     // Loop must iterate even num of times so move shows in the end.
        //     for (int i = 0; i < 8; i++)
        //     {
        //         Thread.Sleep(100);
        //         board.MarkMove(new Move(move.Position, alternate[i % 2]));
        //         board.Print();
        //         System.Console.WriteLine($"{move.Letter} to: {move.Position}.");
        //     }
        // }
    }

    class Move
    {
        public int Position {get; set;}
        public string Letter {get; set;}

        public Move(int position, string letter)
        {
            Position = position;
            Letter = letter;
        }
    }

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

        public void BlinkMove(Move move)
        {
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
            List<int> moves = new List<int>();

            for (int i = 1; i <= boardArea; i++)
                if (IsLegalMove(i)) moves.Add(i);

            return moves;
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

    class Player
    {
        public Board? GameBoard { get; set; }
        public string? Name { get; set; }
        public string? Letter { get; set; }
        public virtual Move Move() => null;
    }

    class HumanPlayer : Player
    {
        public HumanPlayer(Board board, string letter)
        {
            GameBoard = board;
            // Name = name;
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

    class MiniMaxPlayer : Player
    {
        private int movePosition;
        private string x = "X";
        private string o = "O";
        private string otherLetter() => Letter == x ? o : x;

        public MiniMaxPlayer(Board board, string letter)
        {
            GameBoard = board;
            Name = "Computer";
            Letter = letter;
        }

        public override Move Move()
        {
            System.Console.WriteLine("Computer is thinking...");
            MiniMax(GameBoard.Clone());
            return new Move(movePosition, Letter);
        }

        // Recursive algorithm to find best move. 
        private int MiniMax(Board board, bool isMyTurn = true)
        {
            bool isWin = board.IsWin();
            // If game is over, returning score. (Flipping turn because
            // if there is a winner, it's the previous player.)
            if (isWin || board.NoPositions())
                return Score(isWin, !isMyTurn);

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
                scores.Add(MiniMax(futureBoard, !isMyTurn));
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

        // Returns 10 points for a win, -10 for a loss, 0 for a draw.
        private int Score(bool isWin, bool wasMyTurn)
        {
            if (isWin)
            {
                if (wasMyTurn) return 10;
                return -10;
            }
            return 0;
        }
    }

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