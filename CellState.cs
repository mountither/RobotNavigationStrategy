using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigation
{
    /*
     * This class represents the state of the cell when trigger by a search startegy.
     * the cell state must hold details about the parent cell (successeded from)
     * the cell characteristics are held here. 
     */
    class CellState : IComparable<CellState>
    {
        private CellState fParentCell;
        private double fCost;
        private Cell fCellData;
        private double fElapsedDistance;

        public CellState(CellState aParentCell, Cell aCellData)
        {
            fParentCell = aParentCell;
            fCellData = aCellData;
        }
        public CellState(CellState aParentCell, Cell aCellData, double aCost)
        {
            fParentCell = aParentCell;
            fCellData = aCellData;
            fCost = aCost;
        }
    
        // sum of all the cost. depends on search strat
        public double Cost
        {
            get
            {
                return fCost;
            }
            set
            {
                fCost = value;
            }
        }
        
        // g score, e.g used by ASTAR
        public double ElapsedDistance
        {
            get
            {
                return fElapsedDistance;
            }
            set
            {
                fElapsedDistance = value;
            }
        }

        public Cell Data
        {
            get
            {
                return fCellData;
            }
            set
            {
                fCellData = value;
            }
        }

        public CellState ParentCell
        {
            get
            {
                return fParentCell;
            }

        }
        // this method is called upon in PriorityQueue, used to order the Cost in ascending order. 
        public int CompareTo(CellState other)
        {
            if (Cost < other.Cost)
            {
                return -1;
            }
            return 1;
        }
    }
}
