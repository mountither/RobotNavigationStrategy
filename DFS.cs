using System;
using System.Collections.Generic;
using System.Linq;


namespace RobotNavigation
{
    class DFS
    {
        // a stack utilises a LIFO approach, suitable for DFS 
        private Stack<CellState> fStack;
        // must store the vistied cells in a hashset to avoid duplicates
        private HashSet<Cell> fVisited;

        private NavigationPlan fPlan;

        private int fDiscovered;
        private int fSearched;


        public DFS(NavigationPlan aPlan)
        {
            fStack = new Stack<CellState>();
            fVisited = new HashSet<Cell>();

            fPlan = aPlan;
            fDiscovered = 0;
            fSearched = 0;
        }

        public void search(CellState aInitState)
        {
     
            fStack.Push(aInitState);

            CellState lCellState = null;


            while (fStack.Count > 0)
            {
                // remove the first elem in stack and allocate in lCellState. 
                lCellState = fStack.Pop();

                fSearched++;

                if (fPlan.isSolved(lCellState))
                {
                    break;
                }

                if (fVisited.Contains(lCellState.Data))
                    continue;

                // add the current cell as visited. 
                fVisited.Add(lCellState.Data);

                // get the neighbouring cells and add to the frontier for expansion. 
                addCellToFrontier(fPlan.getMoveSet(lCellState), lCellState);

            }

            fPlan.outputResult(lCellState, fSearched, fDiscovered);

        }

        private void addCellToFrontier(List<Cell> aCellList, CellState aParent)
        {
            // loop thru the list of cells that are adjacent to the current cell. 
            // push a cell state for each, with parent and cell data. 
            // the push is at top of stack.
           
            // We need to reverse the list so the first (up direction)
            // is push last (for LIFO operation later)
            aCellList.Reverse();

            foreach (Cell cell in aCellList)
            {

                // duplicates must be removed here, to avoid 'back and forth' visits
                if (!fVisited.Contains(cell))
                {
                    fStack.Push(new CellState(aParent, cell));
                    fDiscovered++;
                }

            }

        }
    }
}
