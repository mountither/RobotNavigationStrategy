using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotNavigation
{
    /*
     * Must implement a heuristic funciton cell's DistanceToGoal plus the distance away from the initail node.
     */
    class ASTAR
    {
        // must consider the two costs (goal distance + 'the distance elapsed') in the priority queue. 
        private PriorityQueue<CellState> fFrontier;
        // keep a record of duplicate cells. 
        private HashSet<Cell> fVisited;
        private NavigationPlan fPlan;

        // use dictionary to maintain the lineage of cellstates and
        // their newly retrieved gscore based on the elapsed distance from the init State
        private Dictionary<CellState, double> fGScoreMap;

        private int fDiscovered;
        private int fSearched;
        private double fEvalFunction;

        public ASTAR(NavigationPlan aPlan)
        {
            fFrontier = new PriorityQueue<CellState>();
            fVisited = new HashSet<Cell>();
            fPlan = aPlan;

            fGScoreMap = new Dictionary<CellState, double>();

            fEvalFunction = 0;
            fDiscovered = 0;
            fSearched = 0;

        }

        public void search(CellState aInitState)
        {
            fFrontier.Enqueue(aInitState);

            CellState lCellState = null;

            // add initial cellstate and gscore
            fGScoreMap.Add(aInitState, 0);

            while (fFrontier.Count() > 0)
            {

                lCellState = fFrontier.Dequeue();

                fSearched++;

                if (fPlan.isSolved(lCellState))
                {
                    break;
                }

                if (fVisited.Contains(lCellState.Data))
                {
                    continue;
                }

                // add to visited nodes hashset
                fVisited.Add(lCellState.Data);
                // the count of visted hashset is the amount of expanded 


                addCellToFrontier(fPlan.getMoveSet(lCellState), lCellState);


            }

            fPlan.outputResult(lCellState, fSearched, fDiscovered);

        }

        private void addCellToFrontier(List<Cell> aCellList, CellState aParent)
        {

            // increment when a cell (aParent) is expanded. To be used as ElapsedDistance in eval func

            //aParent.ElapsedDistance = fExpanded;
            //fExpanded = fVisited.First().DistanceToNext + fExpanded;
            
            //The distance elapsed from the first cell must be summed as the children as searched and chosen
            // I.E: the parent's elapsed distance should be stored. this is then used to + 1 (the current distance to child)
            //aParent.ElapsedDistance = 

            foreach (Cell cell in aCellList)
            {

                if (!fVisited.Contains(cell))
                {
                    // get the gScore value from the parent (current cell) - the elapsed distance.
                    fGScoreMap.TryGetValue(aParent, out double lIntFromDict);

                    // tentative gscore is the distance from intial cell to the neighbour through the current cell (1). 
                    double lTentativeGScore = lIntFromDict + cell.DistanceToNext;

                    // fEval function is the new cost for cell state in ASTAR. The distance to Goal (h(n)) and tentative gscore is added. 
                    fEvalFunction = cell.DistanceToGoal + lTentativeGScore;
                    
                    CellState lNewCellState = new CellState(aParent, cell, fEvalFunction);
                    fFrontier.Enqueue(lNewCellState);

                    fGScoreMap.Add(lNewCellState, lTentativeGScore);

                    fDiscovered++;

                }


            }

        }

    }
}
