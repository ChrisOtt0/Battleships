using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    abstract class Board
    {
        // Field
        protected string[,] displayBoard = new string[10, 10];
        protected string[,] hiddenBoard = new string[10, 10];

        protected bool aircraftCarrierSunk = false;
        protected bool battleshipSunk = false;
        protected bool destroyerSunk = false;
        protected bool submarineSunk = false;
        protected bool patrolBoatSunk = false;


        // Properties
        public string[,] DisplayBoard { get => this.displayBoard; set => this.displayBoard = value; }
        public string[,] HiddenBoard { get => this.hiddenBoard; set => this.hiddenBoard = value; }

        public bool AircraftCarrierSunk { get => this.aircraftCarrierSunk; set => this.aircraftCarrierSunk = value; }
        public bool BattleshipSunk { get => this.battleshipSunk; set => this.battleshipSunk = value; }
        public bool DestroyerSunk { get => this.destroyerSunk; set => this.destroyerSunk = value; }
        public bool SubmarineSunk { get => this.submarineSunk; set => this.submarineSunk = value; }
        public bool PatrolBoatSunk { get => this.patrolBoatSunk; set => this.patrolBoatSunk = value; }


        // Methods
        public void InitiateHiddenBoard()
        {
            for (int i = 0; i < this.hiddenBoard.GetLength(0); i++)
            {
                for (int j = 0; j < this.hiddenBoard.GetLength(1); j++)
                {
                    this.hiddenBoard[i, j] = "·";
                }
            }
        }
        public void InitiateDisplayBoard()
        {
            for (int i = 0; i < this.displayBoard.GetLength(0); i++)
            {
                for (int j = 0; j < this.displayBoard.GetLength(1); j++)
                {
                    this.displayBoard[i, j] = "·";
                }
            }
        }

        virtual public void FillBoard(string[,] board, int difficulty) { }
        virtual public void FillBoard(string[,] playerBoard, string[,] aiBoard, int turn) { }

        virtual public void PlayTurn(string[,] playerBoard) { }
        virtual public void PlayTurn(string[,] hiddenBoard, string[,] displayBoard) { }
    }
}
