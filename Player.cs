using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Player : Board
    {
        public override void FillBoard(string[,] playerBoard, string[,] aiBoard, int turn)
        {
            GameTools.PrintBoards(playerBoard, aiBoard, turn);
            PlaceShip(playerBoard, aiBoard, turn, "aircraft carrier");

            GameTools.PrintBoards(playerBoard, aiBoard, turn);
            PlaceShip(playerBoard, aiBoard, turn, "battleship");

            for (int i = 0; i < 2; i++)
            {
                GameTools.PrintBoards(playerBoard, aiBoard, turn);
                PlaceShip(playerBoard, aiBoard, turn, "cruiser");
            }

            GameTools.PrintBoards(playerBoard, aiBoard, turn);
            PlaceShip(playerBoard, aiBoard, turn, "submarine");

            for (int i = 0; i < 2; i++)
            {
                GameTools.PrintBoards(playerBoard, aiBoard, turn);
                PlaceShip(playerBoard, aiBoard, turn, "destroyer");
            }

            GameTools.PrintBoards(playerBoard, aiBoard, turn);
        }
        public override void PlayTurn(string[,] hiddenBoard, string[,] displayBoard)
        {
            bool success = false;
            int x = 0;
            int y = 0;

            while (!success)
            {
                string coordinates = AttackPoint();
                x = GameTools.ConvertToX(coordinates);
                y = GameTools.ConvertToY(coordinates);

                if (hiddenBoard[x, y] == "I" || hiddenBoard[x, y] == "·")
                    success = true;
                else
                {
                    Console.WriteLine("You have already attacked this position. Try again.\n");
                    success = false;
                }
            }
            Attack(hiddenBoard, displayBoard, x, y);
        }

        private void PlaceShip(string[,] playerBoard, string[,] aiBoard, int turn, string shiptype)
        {
            bool placed = false;

            do
            {
                string coordinates = BoardTools.StartPlayerShip(shiptype);
                int direction = BoardTools.GetDirection();
                int steps = BoardTools.ShiptypeToSteps(shiptype);
                int x = GameTools.ConvertToX(coordinates);
                int y = GameTools.ConvertToY(coordinates);

                if (BoardTools.IsFreeSpace(playerBoard, steps, direction, x, y))
                {
                    BoardTools.PlaceShip(playerBoard, steps, direction, x, y);
                    placed = true;
                }
                else
                {
                    Console.WriteLine("The chosen placement does not work. Please try again.\n");
                    placed = false;
                }
            } while (!placed);
        }
        private string AttackPoint()
        {
            string returnString = "";
            string answer = "";
            bool success = false;

            do
            {
                Console.Write("\nEnter the coordinate you want to attack: ");
                answer = Console.ReadLine().ToUpper();

                if (GameTools.IsValidCoordinate(answer))
                {
                    success = true;
                }

                else
                {
                    Console.WriteLine("Coordinate was not valid. Try again.\n");
                    success = false;
                }
            } while (!success);

            returnString = answer;

            return returnString;
        }
        private void Attack(string[,] hBoard, string[,] dBoard, int x, int y)
        {
            if (hBoard[x, y] == "I")
            {
                hBoard[x, y] = "X";
                dBoard[x, y] = "X";
            }
            else
            {
                hBoard[x, y] = "o";
                dBoard[x, y] = "o";
            }
        }
    }
}
