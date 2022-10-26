namespace TicTacToe
{
    class Game
    {
        private Board board;
        private IPlayer[] players;
        private IPlayer currPlayer;
        private bool isWin = false;
        private bool gameIsOver = false;

        public Game()
        {
            board = new Board();
            board.Init();
            players = GetPlayers();
            currPlayer = players[0];
        }

        public void Run()
        {
            board.Print();

            for (int turn = 0; !gameIsOver && turn < board.Area; turn++)
            {
                currPlayer = GetCurrPlayer(turn);
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

        private IPlayer GetCurrPlayer(int turn) =>
            players[turn % players.Count()];

        private IPlayer[] GetPlayers()
        {
            const string X = "X";
            const string O = "O";
            IPlayer player1 = new HumanPlayer(board, X);
            IPlayer player2 = new HumanPlayer(board, O);

            switch (ReadChoice("Would you like to play a person, or the computer?",
                               "Play a person",
                               "Play the computer"))
            {
                case 1:
                    player1.Name = ReadName("Please enter the name of player 1:");
                    player2.Name = ReadName("Please enter the name of player 2:");
                    break;
                case 2:
                    switch (ReadChoice("Please select a level:",
                                       "Easy (moves randomly)",
                                       "Hard (uses minimax algorithm to play to at least a draw)"))
                    {
                        case 1:
                            player2 = new RandomPlayer(board, O);
                            break;
                        case 2:
                            player2 = new MiniMaxPlayer(board, O);
                            break;
                    }

                    player1.Name = ReadName("Please enter your name:");
                    break;
            }

            Console.Clear();

            if (ReadChoice($"{player1.Name}, would you like to be {X}, or {O}? ({X} goes first.)",
                                X, O) == 1)
                return new IPlayer[] { player1, player2 };

            // If player 1 chose O, changing letters and order of players:
            player1.Letter = O;
            player2.Letter = X;
            return new IPlayer[] { player2, player1 };
        }

        public static int ReadChoice(string prompt, string choice1, string choice2)
        {
            int[] validChoices = { 1, 2 };
            System.Console.WriteLine(prompt);
            System.Console.WriteLine($"1. {choice1}.");
            System.Console.WriteLine($"2. {choice2}.");
            int.TryParse(Console.ReadLine(), out int choice);

            while (!validChoices.Contains(choice))
            {
                System.Console.WriteLine("Sorry, that is an invalid choice. Please enter '1' or '2'.");
                int.TryParse(Console.ReadLine(), out choice);
            }

            Console.Clear();
            return choice;
        }

        private static string ReadName(string prompt)
        {
            string? name;

            do
            {
                System.Console.WriteLine(prompt);
                name = Console.ReadLine();
                if (name != null) name = name.Trim();
            } while (string.IsNullOrEmpty(name));

            return name;
        }
    }
}