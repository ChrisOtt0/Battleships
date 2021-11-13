using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Battleships
{
    class GameTools
    {
        public static int InitiateTurnNr()
        {
            return 1;
        }

        public static void InititateAIBoards(AI ai, int difficulty)
        {
            ai.InitiateDisplayBoard();
            ai.InitiateHiddenBoard();
            ai.FillBoard(ai.HiddenBoard, difficulty);
        }

        public static void InitiatePlayerBoards(Player player, AI ai, int turnNr)
        {
            // Change ai.DisplayBoard to ai.HiddenBoard to debug ai placement
            player.InitiateDisplayBoard();
            player.InitiateHiddenBoard();
            player.FillBoard(player.DisplayBoard, ai.DisplayBoard, turnNr);
        }

        public static bool FlipCoin(Player player, AI ai, int turnNr)
        {
            bool returnBool = false;
            bool success = false;

            PrintBoards(player.DisplayBoard, ai.DisplayBoard, turnNr);
            Console.WriteLine("\t\t\t\t(H)eads\t\tor\t  (T)ails?");

            do
            {
                ConsoleKeyInfo k = Console.ReadKey();

                switch (k.Key)
                {
                    case ConsoleKey.H:
                        returnBool = true;
                        success = true;
                        break;

                    case ConsoleKey.T:
                        returnBool = false;
                        success = true;
                        break;
                }
            } while (!success);

            return returnBool;
        }

        public static void DisplayFirstPlayer(bool coinToss)
        {
            if (coinToss)
            {
                Console.WriteLine("\nPlayer first:");
                System.Threading.Thread.Sleep(1200);
            }
            else
            {
                Console.WriteLine("\nAI first:");
                System.Threading.Thread.Sleep(1200);
            }
        }

        public static bool TurnPlayer(Player player, AI ai, int turnNr)
        {
            bool returnBool = false;

            player.PlayTurn(ai.HiddenBoard, ai.DisplayBoard);
            GameTools.PrintBoards(player.DisplayBoard, ai.DisplayBoard, turnNr);
            returnBool = GameTools.CheckForEndGame(ai.HiddenBoard);

            return returnBool;
        }

        public static bool AttackOrMenu(Player player, AI ai, int turnNr)
        {
            bool returnBool = true;
            bool success = false;

            PrintBoards(player.DisplayBoard, ai.DisplayBoard, turnNr);
            Console.WriteLine("\t\t\t\t(A)ttack\t\t(M)enu");

            do
            {
                ConsoleKeyInfo k = Console.ReadKey();

                switch (k.Key)
                {
                    case ConsoleKey.A:
                        returnBool = true;
                        success = true;
                        break;

                    case ConsoleKey.M:
                        returnBool = false;
                        success = true;
                        break;
                }
            } while (!success);

            return returnBool;
        }

        public static bool PlayerFirst(Player player, AI ai, int turnNr, ref string winner, string[] paths, bool coinToss)
        {
            bool returnBool = false;

        start:;
            if (AttackOrMenu(player, ai, turnNr))
            {
                if (TurnPlayer(player, ai, turnNr))
                {
                    winner = "Player";
                    returnBool = true;
                }

                if (TurnAI(player, ai, turnNr))
                {
                    winner = "AI";
                    returnBool = true;
                }
            }
            else
            {
                if (InGameMenu(player, ai, turnNr))
                {
                    if (WantToSave(player, ai, turnNr))
                    {
                        SaveGame(player, ai, turnNr, paths, coinToss);
                        winner = "Save";
                        returnBool = true;
                    }
                    else
                    {
                        winner = "Save";
                        returnBool = true;
                    }
                }
                else
                    goto start;
            }

            return returnBool;
        }

        public static bool AIFirst(Player player, AI ai, int turnNr, ref string winner, string[] paths, bool coinToss)
        {
            bool returnBool = false;

            if (GameTools.TurnAI(player, ai, turnNr))
            {
                winner = "AI";
                returnBool = true;
            }

        start:;
            if (AttackOrMenu(player, ai, turnNr))
            {
                if (TurnPlayer(player, ai, turnNr))
                {
                    winner = "Player";
                    returnBool = true;
                }
            }
            else
            {
                if (InGameMenu(player, ai, turnNr))
                {
                    if (WantToSave(player, ai, turnNr))
                    {
                        SaveGame(player, ai, turnNr, paths, coinToss);
                        winner = "Save";
                        returnBool = true;
                    }
                    else
                    {
                        winner = "Save";
                        returnBool = true;
                    }
                }
                else
                    goto start;
            }

            return returnBool;
        }

        public static bool TurnAI(Player player, AI ai, int turnNr)
        {
            bool returnBool = false;

            ai.PlayTurn(player.DisplayBoard);
            GameTools.PrintBoards(player.DisplayBoard, ai.DisplayBoard, turnNr);
            returnBool = GameTools.CheckForEndGame(player.DisplayBoard);

            return returnBool;
        }

        public static bool InGameMenu(Player player, AI ai, int turnNr)
        {
            bool returnBool = false;
            bool success = false;

            PrintBoards(player.DisplayBoard, ai.DisplayBoard, turnNr);
            Console.WriteLine("\t\t\t\t(B)ack\t\t\t(Q)uit to Main");

            do
            {
                ConsoleKeyInfo k = Console.ReadKey();

                switch (k.Key)
                {
                    case ConsoleKey.B:
                        returnBool = false;
                        success = true;
                        break;

                    case ConsoleKey.Q:
                        returnBool = true;
                        success = true;
                        break;
                }
            } while (!success);

            return returnBool;
        }

        public static bool WantToSave(Player player, AI ai, int turnNr)
        {
            bool returnBool = false;
            bool success = false;

            PrintBoards(player.DisplayBoard, ai.DisplayBoard, turnNr);
            Console.WriteLine("Do you want to save before quitting? (y/n)");

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

            return returnBool;
        }

        public static void SaveGame(Player player, AI ai, int turnNr, string[] paths, bool coinToss)
        {
            StreamWriter writer = null;
            string forSaving = "";

            try
            {
                writer = new StreamWriter(paths[0]);
                for (int i = 0; i < player.DisplayBoard.GetLength(0); i++)
                    for (int j = 0; j < player.DisplayBoard.GetLength(1); j++)
                    {
                        forSaving = forSaving + player.DisplayBoard[i, j] + "\n";
                    };
                writer.WriteLine(forSaving);
                forSaving = "";
                writer.Close();

                writer = new StreamWriter(paths[1]);
                for (int i = 0; i < player.HiddenBoard.GetLength(0); i++)
                    for (int j = 0; j < player.HiddenBoard.GetLength(1); j++)
                    {
                        forSaving = forSaving + player.HiddenBoard[i, j] + "\n";
                    };
                writer.WriteLine(forSaving);
                forSaving = "";
                writer.Close();

                writer = new StreamWriter(paths[2]);
                for (int i = 0; i < ai.DisplayBoard.GetLength(0); i++)
                    for (int j = 0; j < ai.DisplayBoard.GetLength(1); j++)
                    {
                        forSaving = forSaving + ai.DisplayBoard[i, j] + "\n";
                    };
                writer.WriteLine(forSaving);
                forSaving = "";
                writer.Close();

                writer = new StreamWriter(paths[3]);
                for (int i = 0; i < ai.HiddenBoard.GetLength(0); i++)
                    for (int j = 0; j < ai.HiddenBoard.GetLength(1); j++)
                    {
                        forSaving = forSaving + ai.HiddenBoard[i, j] + "\n";
                    };
                writer.WriteLine(forSaving);
                forSaving = "";
                writer.Close();

                writer = new StreamWriter(paths[4]);
                writer.WriteLine("Turn Nr: " + turnNr);
                writer.WriteLine("CoinToss: " + coinToss);
                writer.Close();
            }
            catch
            {
                Console.WriteLine("Error while saving.");
            }
            finally
            {
                if (writer != null) writer.Close();
            }

        }

        public static int LoadTurnNr(string path)
        {
            int returnInt = 1;
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(path);
                while (reader.Peek() > -1)
                {
                    string buffer = reader.ReadLine();
                    if (buffer.Substring(0, 7) == "Turn Nr")
                    {
                        returnInt = Convert.ToInt32(buffer.Substring(9));
                        break;
                    }
                }
            }
            catch
            {
                returnInt = 1;
            }
            finally
            {
                if (reader != null) reader.Close();
            }

            return returnInt;
        }

        public static bool LoadCoinToss(string path)
        {
            bool returnBool = true;
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(path);
                while (reader.Peek() > -1)
                {
                    string buffer = reader.ReadLine();
                    if (buffer.Substring(0, 8) == "CoinToss")
                    {
                        if (buffer.Substring(10) == "True")
                        {
                            returnBool = true;
                            break;
                        }
                        else
                        {
                            returnBool = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                returnBool = true;
            }
            finally
            {
                if (reader != null) reader.Close();
            }

            return returnBool;
        }

        public static void LoadAIBoards(AI ai, string[] paths)
        {

            try
            {
                LoadBoard(ai.DisplayBoard, paths[2]);
                LoadBoard(ai.HiddenBoard, paths[3]);
            }
            catch
            {
                InititateAIBoards(ai, 1);
            }
        }

        public static void LoadPlayerBoards(Player player, AI ai, int turnNr, string[] paths)
        {
            StreamReader reader = null;

            try
            {
                LoadBoard(player.DisplayBoard, paths[0]);
                LoadBoard(player.HiddenBoard, paths[1]);
            }
            catch
            {
                InitiatePlayerBoards(player, ai, turnNr);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public static void LoadBoard(string[,] board, string path)
        {
            StreamReader reader = null;
            int i = 0;
            int j = 0;

            reader = new StreamReader(path);
            while (reader.Peek() > -1)
            {
                board[i, j] = reader.ReadLine();

                j++;
                if (i == 9 && j == 10)
                    break;
                else if (j == 10)
                {
                    j = 0;
                    i++;
                }
            }
            reader.Close();
        }

        public static void NullifySaves(Player player, AI ai, string[] paths)
        {
            StreamWriter writer = null;

            try
            {
                foreach (var path in paths)
                {
                    writer = new StreamWriter(path);
                    writer.WriteLine("null");
                    writer.Close();
                }
            }
            catch
            {
                Console.WriteLine("Failed at nullifying saves. Please do so manually by entering \"null\" into the save files.");
            }
        }

        public static bool AnotherGame()
        {
            bool returnBool = false;
            bool success = false;

            do
            {
                Console.WriteLine("Do you want to play another game? (y/n)");
                ConsoleKeyInfo answer = Console.ReadKey();

                if (answer.Key == ConsoleKey.Y)
                {
                    returnBool = true;
                    success = true;
                }

                else if (answer.Key == ConsoleKey.N)
                {
                    returnBool = false;
                    success = true;
                }
                else
                    Console.WriteLine("Instructions unclear. Try again.");

            } while (!success);

            return returnBool;
        }

        public static void PrintBoards(string[,] boardPlayer, string[,] boardAI, int turn)
        {
            Console.Clear();
            Console.WriteLine($"\t\tPlayer: 1\t\t     Turn: {turn}\t\t\tPlayer: AI\n");
            Console.WriteLine("     1   2   3   4   5   6   7   8   9   10\t\t   1   2   3   4   5   6   7   8   9   10\n");

            int row = 65;

            for (int i = 0; i < boardPlayer.GetLength(0); i++)
            {
                Console.Write($"{Convert.ToChar(row)}:   ");
                for (int j = 0; j < boardPlayer.GetLength(1); j++)
                {
                    Console.Write($"{boardPlayer[i, j]}   ");
                }
                Console.WriteLine("\n");
                row++;
            }

            row = 65;

            for (int i = 0; i < boardAI.GetLength(0); i++)
            {
                Console.SetCursorPosition(54, (i * 2) + 4);
                Console.Write($"{Convert.ToChar(row)}:   ");
                for (int j = 0; j < boardAI.GetLength(1); j++)
                {
                    Console.Write($"{boardAI[i, j]}   ");
                }
                row++;
            }

            Console.SetCursorPosition(0, 25);
        }

        public static bool IsValidCoordinate(string input)
        {
            bool returnBool = false;
            Regex firstCheck = new Regex(@"^([A-J])");
            Regex secondCheck = new Regex(@"^([0-9])");
            string firstPart = "";
            string secondPart = "";
            if (input != "")
                firstPart = input.Substring(0, 1);
            if (input != "")
                secondPart = input.Substring(1);
            int secondPartInt = 0;

            if (firstCheck.IsMatch(firstPart) && secondCheck.IsMatch(secondPart))
            {
                try
                {
                    secondPartInt = Convert.ToInt32(secondPart);
                    if (secondPartInt > 0 && secondPartInt < 11)
                        returnBool = true;
                }
                catch (Exception)
                {
                    returnBool = false;
                }
            }

            return returnBool;
        }

        public static bool PathExistsAndNotEmpty(string[] paths)
        {
            bool returnBool = true;
            bool exist = true;
            bool peek = true;

            // Exists
            foreach (string path in paths)
                if (!File.Exists(path))
                    exist = false;

            // Not empty
            try
            {
                peek = NotEmpty(paths);
            }
            catch
            {
                returnBool = exist && peek;
            }

            returnBool = exist && peek;

            return returnBool;
        }

        public static bool NotEmpty(string[] paths)
        {
            bool returnBool = true;

            foreach (string path in paths)
            {
                StreamReader reader = new StreamReader(path);
                while (reader.Peek() > -1)
                {
                    string buffer = reader.ReadLine();
                    if (buffer == "null")
                    {
                        returnBool = false;
                    }
                }
                reader.Close();
            }

            return returnBool;
        }

        public static bool PathsAndFilesExist(string[] paths)
        {
            bool returnBool = false;
            StreamWriter writer = null;

            if (!Directory.Exists(paths[0].Substring(0, 47)))
            {
                Directory.CreateDirectory(paths[0].Substring(0, 47));
            }

            foreach (string path in paths)
            {
                if (!File.Exists(path))
                {
                    try
                    {
                        writer = new StreamWriter(path);
                        writer.WriteLine("null");
                        returnBool = true;
                    }
                    catch
                    {
                        returnBool = false;
                    }
                    finally
                    {
                        if (writer != null)
                            writer.Close();
                    }
                }
                else
                {
                    returnBool = true;
                }
            }

            return returnBool;
        }

        public static int ConvertToX(string coordinate)
        {
            int returnInt = 0;

            switch (coordinate[0])
            {
                case 'A':
                    returnInt = 0;
                    break;

                case 'B':
                    returnInt = 1;
                    break;

                case 'C':
                    returnInt = 2;
                    break;

                case 'D':
                    returnInt = 3;
                    break;

                case 'E':
                    returnInt = 4;
                    break;

                case 'F':
                    returnInt = 5;
                    break;

                case 'G':
                    returnInt = 6;
                    break;

                case 'H':
                    returnInt = 7;
                    break;

                case 'I':
                    returnInt = 8;
                    break;

                case 'J':
                    returnInt = 9;
                    break;

            }

            return returnInt;
        }

        public static int ConvertToY(string coordinate)
        {
            int returnInt = 0;

            returnInt = Convert.ToInt32(coordinate.Substring(1)) - 1;

            return returnInt;
        }

        public static bool CheckForEndGame(string[,] currentBoard)
        {
            bool returnBool = true;

            foreach (string coordinate in currentBoard)
            {
                if (coordinate == "I")
                    returnBool = false;
            }

            return returnBool;
        }

        public static void DisplayWinner(string winner)
        {
            Console.SetCursorPosition(108, 12);
            Console.WriteLine("GAME OVER!");
            Console.SetCursorPosition(108, 13);
            if (winner == "Player")
                Console.WriteLine(" YOU WIN!");
            else
                Console.WriteLine(" YOU LOOSE!");

            Console.SetCursorPosition(0, 25);
        }
    }
}
