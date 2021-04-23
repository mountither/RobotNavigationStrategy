using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    /*
     * Depth Limit Search. - An uninformed search strat. 
     * The search method now takes in the depth limit of the tree. 
     */
    class DLS
    {
        private Stack<CellState> fStack;

        private NavigationPlan fPlan;

        private HashSet<Cell> fVisited;

        private int fDepth;
        private int fDiscovered;

        public DLS(NavigationPlan aPlan)
        {
            fPlan = aPlan;
            fStack = new Stack<CellState>();
            fVisited = new HashSet<Cell>();
            fDepth = 0;
            fDiscovered = 0;

        }

        public void search(CellState aInitState, int aDepthLimit)
        {

            fStack.Push(aInitState);

            CellState lCellState = null;
            while (fStack.Count != 0)
            {
                // enter if the current cellState's depth is less than the limit. 
                if(fDepth <= aDepthLimit)
                {
                    // remove the first elem in stack and allocate in lCellState. 
                    lCellState = fStack.Pop();

                    // increment the depth of tree, since we are at child cell.
                    
                    if (fPlan.isSolved(lCellState))
                    {
                        break;
                    }

                    if (fVisited.Contains(lCellState.Data))
                        continue;


                    fVisited.Add(lCellState.Data);
                    fDepth++;

                    addCellToFrontier(fPlan.getMoveSet(lCellState), lCellState);
                }
                else
                {
                    Console.WriteLine("The Goal Cell was not Found within the given Depth Limit.");
                }
            }

            fPlan.outputResult(lCellState, fDepth, fDiscovered);

        }

        public void addCellToFrontier(List<Cell> aCellList, CellState aParent)
        {
            foreach (Cell cell in aCellList)
            {
                if (!fVisited.Contains(cell))
                {
                    fStack.Push(new CellState(aParent, cell));
                    fDiscovered++;

                }

            }

        }
    }
}
