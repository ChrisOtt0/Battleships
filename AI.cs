using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class AI : Board
    {
        // Methods
        public override void FillBoard(string[,] board, int difficulty)
        {
            switch (difficulty)
            {
                case 0:
                    FillShip(board, "aircraftCarrier");
                    FillShip(board, "battleship");
                    FillShip(board, "cruiser");
                    FillShip(board, "destroyer");
                    break;

                case 1:
                    FillShip(board, "aircraftCarrier");
                    FillShip(board, "battleship");
                    for (int i = 0; i < 2; i++)
                        FillShip(board, "cruiser");
                    FillShip(board, "submarine");
                    for (int i = 0; i < 2; i++)
                        FillShip(board, "destroyer");
                    break;

                case 2:
                    FillShip(board, "aircraftCarrier");
                    for (int i = 0; i < 2; i++)
                        FillShip(board, "battleship");
                    for (int i = 0; i < 3; i++)
                        FillShip(board, "cruiser");
                    FillShip(board, "submarine");
                    for (int i = 0; i < 4; i++)
                        FillShip(board, "destroyer");
                    break;
            }
        }
        public override void PlayTurn(string[,] playerBoard)
        {
            Random randX = new Random();
            Random randY = new Random();
            bool success = false;
            int x = 0;
            int y = 0;

            while (!success)
            {
                x = randX.Next(0, 10);
                y = randY.Next(0, 10);

                if (playerBoard[x, y] == "I")
                {
                    playerBoard[x, y] = "X";
                    success = true;
                }
                else if (playerBoard[x, y] == "·")
                {
                    playerBoard[x, y] = "o";
                    success = true;
                }
                else
                    success = false;
            }
        }

        private void FillShip(string[,] board, string shiptype)
        {
            Random randX = new Random();
            Random randY = new Random();
            Random randDir = new Random();
            bool placed = false;
            int steps = 0;

            steps = BoardTools.ShiptypeToSteps(shiptype);

            while (!placed)
            {
                int currentX = randX.Next(0, 10);
                int currentY = randY.Next(0, 10);
                int direction = randDir.Next(0, 4);

                bool isFreeSpace = BoardTools.IsFreeSpace(board, steps, direction, currentX, currentY);
                if (isFreeSpace)
                {
                    BoardTools.PlaceShip(board, steps, direction, currentX, currentY);
                    placed = true;
                }
                else
                    placed = false;
            }
        }

    }
}
