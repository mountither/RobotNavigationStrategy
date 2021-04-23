using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotNavigation
{
    /*
     * Must implement a heuristic funciton cell's DistanceToGoal.
     */
    class GBFS
    {
        // custom priorityQueue is used to record the distince cost (heuristic) and order ascending. 
        private PriorityQueue<CellState> fFrontier;
        // keep a record of visited cells. 
        private HashSet<Cell> fVisited;

        private NavigationPlan fPlan;
        private int fDiscovered;
        private int fSearched;


        public GBFS(NavigationPlan aPlan)
        {
            fFrontier = new PriorityQueue<CellState>();
            fVisited = new HashSet<Cell>();

            fPlan = aPlan;

            fDiscovered = 0;
            fSearched = 0;
        }

        public void search(CellState aInitState)
        {
            fFrontier.Enqueue(aInitState);

            CellState lCellState = null;

            while (fFrontier.Count() > 0)
            {

                lCellState = fFrontier.Dequeue();

                fSearched++;

                if (fPlan.isSolved(lCellState))
                {
                    break;
                }

                if (fVisited.Contains(lCellState.Data))
                    continue;

                // add to visited nodes hashset
                fVisited.Add(lCellState.Data);

                addCellToFrontier(fPlan.getMoveSet(lCellState), lCellState);

            }

            fPlan.outputResult(lCellState, fSearched, fDiscovered);

        }

        private void addCellToFrontier(List<Cell> aCellList, CellState aParent)
        {
            foreach (Cell cell in aCellList)
            {
                if (!fVisited.Contains(cell))
                {
                    fFrontier.Enqueue(new CellState(aParent, cell, cell.DistanceToGoal));
                    fDiscovered++;
                }
            }

        }
    }
}
