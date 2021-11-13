using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Menus
    {
        public static int MainMenu()
        {
            int returnInt = -1;

            PrintBanner();
            Console.Write("\n   (N)ew Game\t   (C)ontinue\t   (O)ptions\t   (E)xit");

            returnInt = KeyToInt();

            return returnInt;
        }
        public static bool AreYouSure()
        {
            bool returnBool = false;
            bool success = false;

            PrintBanner();
            Console.Write("\n\tAre you sure you want to exit? (y/n)");

            do
            {
                ConsoleKeyInfo k = Console.ReadKey();

                switch (k.Key)
                {
                    case ConsoleKey.Y:
                        returnBool = true;
                        success = true;
                        break;

                    case ConsoleKey.N:
                        returnBool = false;
                        success = true;
                        break;
                }
            } while (!success);

            Console.WriteLine("\n");

            return returnBool;
        }
        public static bool NewGame(int difficulty, string[] paths)
        {
            bool returnBool = true;
            Random rand = new Random();
            bool savesCreated = false;

            savesCreated = GameTools.PathsAndFilesExist(paths);
            if (savesCreated)
            {
                do
                {
                    AI ai = new AI();
                    Player player = new Player();
                    string winner = "";
                    bool winnerFound = false;
                    int turnNr = GameTools.InitiateTurnNr();
                    bool PlayerIsHeads = false;
                    bool isHeads = false;

                    GameTools.InititateAIBoards(ai, difficulty);
                    GameTools.InitiatePlayerBoards(player, ai, turnNr);
                    PlayerIsHeads = GameTools.FlipCoin(player, ai, turnNr);
                    isHeads = (rand.Next(0, 2) == 1);
                    bool coinToss = PlayerIsHeads == isHeads;
                    GameTools.DisplayFirstPlayer(coinToss);

                    do
                    {
                        if (coinToss)
                            winnerFound = GameTools.PlayerFirst(player, ai, turnNr, ref winner, paths, coinToss);

                        else
                            winnerFound = GameTools.AIFirst(player, ai, turnNr, ref winner, paths, coinToss);

                        if (winner == "Save")
                            break;
                    } while (!winnerFound);

                    if (winner == "Player" || winner == "AI")
                        GameTools.DisplayWinner(winner);
                    else
                        break;
                } while (GameTools.AnotherGame());
            }
            else
            {
                PrintBanner();
                Console.WriteLine("\nCould not create savefiles. Try manually checking in your home directory for:"
                    + "\nDocuments\\Games\\Battleship\\save\n\nThese files should be present:"
                    + "\nplayerDisplayBoard.txt\nplayerHiddenBoard.txt\naiDisplayBoard.txt\naiHiddenBoard.txt\nsettings.txt");
            }

            // Still needs implementation of exit to main or exit to desktop

            return returnBool;
        }
        public static int SetDifficulty()
        {
            int returnInt = -1;
            bool success = false;

            PrintBanner();
            Console.WriteLine("\n\t\t\tSet difficulty:\n\t   (E)asy\t   (M)edium\t    (H)ard");

            do
            {
                ConsoleKeyInfo k = Console.ReadKey();

                switch (k.Key)
                {
                    case ConsoleKey.E:
                        returnInt = 0;
                        success = true;
                        break;

                    case ConsoleKey.M:
                        returnInt = 1;
                        success = true;
                        break;

                    case ConsoleKey.H:
                        returnInt = 2;
                        success = true;
                        break;
                }
            } while (!success);

            return returnInt;
        }
        public static bool ContinueGame(string[] paths)
        {
            bool returnBool = false;

            AI ai = new AI();
            Player player = new Player();
            string winner = "";
            bool winnerFound = false;
            bool coinToss = GameTools.LoadCoinToss(paths[4]);
            int turnNr = GameTools.LoadTurnNr(paths[4]);
            GameTools.LoadAIBoards(ai, paths);
            GameTools.LoadPlayerBoards(player, ai, turnNr, paths);

            do
            {
                if (coinToss)
                    winnerFound = GameTools.PlayerFirst(player, ai, turnNr, ref winner, paths, coinToss);

                else
                    winnerFound = GameTools.AIFirst(player, ai, turnNr, ref winner, paths, coinToss);

                if (winner == "Save")
                    break;
            } while (!winnerFound);

            if (winner == "Player" || winner == "AI")
            {
                GameTools.DisplayWinner(winner);
                GameTools.NullifySaves(player, ai, paths);
                returnBool = GameTools.AnotherGame();
            }
            else
            {
                returnBool = false;
            }

            return returnBool;
        }
        public static bool NoSavedGame()
        {
            bool returnBool = true;
            bool success = false;

            do
            {
                PrintBanner();
                Console.Write("\nThere is no saved game. Exit to (M)ain Menu or Exit to (D)esktop?");

                ConsoleKeyInfo k = Console.ReadKey();

                switch (k.Key)
                {
                    case ConsoleKey.M:
                        returnBool = true;
                        success = true;
                        break;

                    case ConsoleKey.D:
                        returnBool = false;
                        success = true;
                        break;
                }
            } while (!success);

            Console.WriteLine("\n");

            return returnBool;
        }

        private static void PrintBanner()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(@"  __ )          |    |    |              |     _)              ");
            Console.WriteLine(@"  __ \    _` |  __|  __|  |   _ \   __|  __ \   |  __ \    __| ");
            Console.WriteLine(@"  |   |  (   |  |    |    |   __/ \__ \  | | |  |  |   | \__ \ ");
            Console.WriteLine(@" ____/  \__,_| \__| \__| _| \___| ____/ _| |_| _|  .__/  ____/ ");
            Console.WriteLine(@"                                                  _|           ");
        }
        private static int KeyToInt()
        {
            int returnInt = -1;
            bool success = false;

            do
            {
                ConsoleKeyInfo k = Console.ReadKey();

                switch (k.Key)
                {
                    case ConsoleKey.N:
                        returnInt = 0;
                        success = true;
                        break;

                    case ConsoleKey.C:
                        returnInt = 1;
                        success = true;
                        break;

                    case ConsoleKey.O:
                        returnInt = 2;
                        success = true;
                        break;

                    case ConsoleKey.E:
                        returnInt = 3;
                        success = true;
                        break;
                }
            } while (!success);

            return returnInt;
        }
    }
}
