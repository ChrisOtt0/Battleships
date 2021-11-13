using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class BoardTools
    {
        public static bool IsFreeSpace(string[,] board, int steps, int direction, int x, int y)
        {
            bool returnBool = false;

            switch (direction)
            {
                case 0:
                    if (x - (steps - 1) >= 0)
                        returnBool = IsFreeNorth(board, steps, x, y);
                    break;

                case 1:
                    if (x + (steps - 1) <= 9)
                        returnBool = IsFreeSouth(board, steps, x, y);
                    break;

                case 2:
                    if (y - (steps - 1) >= 0)
                        returnBool = IsFreeWest(board, steps, x, y);
                    break;

                case 3:
                    if (y + (steps - 1) <= 9)
                        returnBool = IsFreeEast(board, steps, x, y);
                    break;
            }

            return returnBool;
        }

        private static bool IsFreeNorth(string[,] board, int steps, int x, int y)
        {
            bool returnBool = false;
            bool check = true;

            for (int i = 0; i < steps; i++)
            {
                if (board[x - i, y] == "I")
                {
                    check = false;
                }
            }

            if (check)
                returnBool = true;
            else
                returnBool = false;

            return returnBool;
        }
        private static bool IsFreeSouth(string[,] board, int steps, int x, int y)
        {
            bool returnBool = false;
            bool check = true;

            for (int i = 0; i < steps; i++)
            {
                if (board[x + i, y] == "I")
                {
                    check = false;
                }
            }

            if (check)
                returnBool = true;
            else
                returnBool = false;

            return returnBool;
        }
        private static bool IsFreeWest(string[,] board, int steps, int x, int y)
        {
            bool returnBool = false;
            bool check = true;

            for (int i = 0; i < steps; i++)
            {
                if (board[x, y - i] == "I")
                {
                    check = false;
                }
            }

            if (check)
                returnBool = true;
            else
                returnBool = false;

            return returnBool;
        }
        private static bool IsFreeEast(string[,] board, int steps, int x, int y)
        {
            bool returnBool = false;
            bool check = true;

            for (int i = 0; i < steps; i++)
            {
                if (board[x, y + i] == "I")
                {
                    check = false;
                }
            }

            if (check)
                returnBool = true;
            else
                returnBool = false;

            return returnBool;
        }


        public static void PlaceShip(string[,] board, int steps, int direction, int x, int y)
        {
            switch (direction)
            {
                case 0:
                    PlaceNorth(board, steps, x, y);
                    break;

                case 1:
                    PlaceSouth(board, steps, x, y);
                    break;

                case 2:
                    PlaceWest(board, steps, x, y);
                    break;

                case 3:
                    PlaceEast(board, steps, x, y);
                    break;
            }
        }

        private static void PlaceNorth(string[,] board, int steps, int x, int y)
        {
            for (int i = 0; i < steps; i++)
            {
                board[x - i, y] = "I";
            }
        }
        private static void PlaceSouth(string[,] board, int steps, int x, int y)
        {
            for (int i = 0; i < steps; i++)
            {
                board[x + i, y] = "I";
            }
        }
        private static void PlaceWest(string[,] board, int steps, int x, int y)
        {
            for (int i = 0; i < steps; i++)
            {
                board[x, y - i] = "I";
            }
        }
        private static void PlaceEast(string[,] board, int steps, int x, int y)
        {
            for (int i = 0; i < steps; i++)
            {
                board[x, y + i] = "I";
            }
        }

        public static int ShiptypeToSteps(string type)
        {
            int returnInt = 0;

            switch (type)
            {
                case "aircraftCarrier":
                    returnInt = 5;
                    break;

                case "aircraft carrier":
                    returnInt = 5;
                    break;

                case "battleship":
                    returnInt = 4;
                    break;

                case "cruiser":
                    returnInt = 3;
                    break;

                case "submarine":
                    returnInt = 3;
                    break;

                case "destroyer":
                    returnInt = 2;
                    break;
            }

            return returnInt;
        }

        public static string StartPlayerShip(string shiptype)
        {
            string returnString = "";
            string answer = "";
            string shipName = shiptype[0].ToString().ToUpper() + shiptype.Substring(1);
            bool success = false;

            do
            {
                Console.Write($"Enter the start coordinate for your {shiptype}: ");
                answer = Console.ReadLine().ToUpper();

                if (GameTools.IsValidCoordinate(answer))
                    success = true;
                else
                {
                    Console.WriteLine("Entered coordinate isn't valid. Please try again.\n");
                    success = false;
                }

            } while (!success);

            returnString = answer;

            return returnString;
        }

        public static int GetDirection()
        {
            int returnInt = 0;
            bool success = false;
            string answer = "";

            while (!success)
            {
                Console.Write("Enter the facing direction of the ship (north / south / west / east): ");
                answer = Console.ReadLine().ToLower().Substring(0, 1);

                switch (answer)
                {
                    case "n":
                        returnInt = 0;
                        success = true;
                        break;

                    case "s":
                        returnInt = 1;
                        success = true;
                        break;

                    case "w":
                        returnInt = 2;
                        success = true;
                        break;

                    case "e":
                        returnInt = 3;
                        success = true;
                        break;

                    default:
                        Console.WriteLine("Instruction unclear. Please try again.\n");
                        success = false;
                        break;
                }
            }

            return returnInt;
        }
    }
}
