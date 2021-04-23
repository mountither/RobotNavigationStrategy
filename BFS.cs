using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotNavigation
{
    class BFS
    {
        private LinkedList<CellState> fFrontier;
        // keep a record of duplicate cells. 
        private HashSet<Cell> fVisited;

        private NavigationPlan fPlan;

        private int fDiscovered;
        private int fSearched;


        public BFS(NavigationPlan aPlan)
        {
            fFrontier = new LinkedList<CellState>();
            fVisited = new HashSet<Cell>();

            fPlan = aPlan;
            fDiscovered = 0;
            fSearched = 0;
        }

        public void search(CellState aInitState)
        {
            fFrontier.AddLast(aInitState);

            CellState lCellState = null;

            while (fFrontier.Count > 0)
            {

                lCellState = fFrontier.First();
                fFrontier.RemoveFirst();

                fSearched++;


                if (fPlan.isSolved(lCellState))
                {
                    break;
                }

                if (fVisited.Contains(lCellState.Data))
                    continue;

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
                    fFrontier.AddLast(new CellState(aParent, cell));

                    fDiscovered++;
                }

            }

        }

    }
}
