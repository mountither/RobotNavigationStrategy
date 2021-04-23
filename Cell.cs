using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigation
{
    /*
     * This class represents each cell on the map. 
     * The cell will hold the contents of its state
     *      - X, Y position
     *      - Empty or not
     *      - list of neighbouring cells
     *      - the parent that cell (allocated when node/cell is expanded)
     *      - The cost of a path. the knowledge of the path cost (eval function) is stored here
     */
    class Cell
    {
        private bool fIsEmpty;
        private List<AdjacentCell> fAdjCells = new List<AdjacentCell>();
        private double fDistanceToGoal;
        // default - 1 unit to neigbouring cell.
        private double fDistanceToNext;

        public Cell(int aX, int aY, double aDistanceToGoal, double aDistanceToNext)
        {
            X = aX;
            Y = aY;
            fIsEmpty = false;
            fDistanceToGoal = aDistanceToGoal;
            fDistanceToNext = aDistanceToNext;
        }
        public int X { get; }

        public int Y { get; }

        public bool IsEmpty
        {
            get
            {
                return fIsEmpty;
            }
            set
            {
                fIsEmpty = value;
            }
        }

        public double DistanceToGoal
        {
            get
            {
                return fDistanceToGoal;
            }
        }
        public double DistanceToNext
        {
            get
            {
                return fDistanceToNext;
            }
        }

        public List<AdjacentCell> AdjacentCells
        {
            get
            {
                return fAdjCells;
            }
            set
            {
                fAdjCells = value;
            }
        }

    }
}
