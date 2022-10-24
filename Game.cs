namespace TicTacToe
{
    class Game
    {
        Board board;
        Player[] players;
        Player currPlayer;
        bool isWin = false;
        bool gameIsOver = false;

        public Game()
        {
            board = new Board();
            board.Init();
            SetPlayers();
        }

        public void Run()
        {
            board.Print();

            for (int turn = 0; !gameIsOver && turn < board.Area; turn++)
            {
                SetCurrPlayer(turn);
                board.ShowMove(currPlayer.Move());
                gameIsOver = isWin = board.IsWin();
            }

            gameIsOver = true;
            System.Console.WriteLine("Game over.");
            if (isWin)
                System.Console.WriteLine($"{currPlayer.Name} wins!");
            else
                System.Console.WriteLine("The game is drawn.");
        }

        public static int GetChoice()
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

        private void SetCurrPlayer(int turn) =>
            currPlayer = players[turn % players.Count()];

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
    }
}