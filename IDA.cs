using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotNavigation
{
    /*
     * Even though has similiar props as Astar, the priority queue is not required.
     * Since it only uses cost comparison when committing down branch(s)
     *  Similar to DLS. But the threshold is replaced with the eval function (f score for cutoff)
     */
    class IDA
    {
        // must consider the two costs (goal distance + 'the distance elapsed') in the priority queue.
        private Stack<CellState> fFrontier;

        // use dictionary to maintain the lineage of cellstates and
        // their newly retrieved gscore based on the elapsed distance from the init State
        private Dictionary<CellState, double> fGScoreMap;

        private NavigationPlan fPlan;
        private int fDiscovered;
        private int fSearched;

        private double fEvalFunction;
        public IDA(NavigationPlan aPlan)
        {
            fFrontier = new Stack<CellState>();
            fGScoreMap = new Dictionary<CellState, double>();

            fPlan = aPlan;

            fEvalFunction = 0;
            fDiscovered = 0;
            fSearched = 0;
        }

        // thershold is the maximum amount from the evaluation function (cuttoff)
        // the threshold is selected based on the DistanceToGoal (heuristic), since the value will 
        // always be less than the value returned from fEvalFunction (distance to goal + distance to next cell) 
        public void search(CellState aInitState)
        {
            List<CellState> lPath = new List<CellState>();

            lPath.Add(aInitState);

            // add initial cellstate and gscore
            fGScoreMap.Add(aInitState, 0);

            // start threshold - from init pos to goal euch dist
            double lCurrentThreshold = aInitState.Data.DistanceToGoal + 0;
            double lSmallestThershold;
            while (lCurrentThreshold != double.MaxValue)
            {
                // store the smallest f score threshold here (from the cuessor cells)

                lSmallestThershold = recursiveSearch(lPath, 0, lCurrentThreshold);

                // goal cell is found 
                if (lSmallestThershold == 0.0)
                {
                    break;
                }

                lCurrentThreshold = lSmallestThershold;
                
            }

            fPlan.outputResult(lPath.Last(), fSearched, fDiscovered);

        }

        // will search down all the successor cells and will prevent searching down cell with higher distance to goal value than the 
        // current threshold. 
        private double recursiveSearch(List<CellState> aPaths, double aGraphCost, double aThreshold)
        {
            CellState lCurrent = aPaths.Last();

            // threshold = g(n) + h(n)

            double aCurrThreshold = aGraphCost + lCurrent.Data.DistanceToGoal;

            //Console.WriteLine(lCurrent.Data.X + " " + lCurrent.Data.Y + " - " + aCurrThreshold);

            fSearched++;

            // add this cell to visited hashset and avoid visting it again.

            // check if the current cell has higher h(n) than the thresh.
            // returns the current distance to goal
            if (aCurrThreshold > aThreshold)
            {
                
                return aCurrThreshold;
            }

            if (fPlan.isSolved(lCurrent))
            {
                return 0;
            }

            double lMinimumIsFound = double.MaxValue;

            // search through the current's cell childern
            List<Cell> lChildren = fPlan.getMoveSet(lCurrent);
            // must reverse to keep the robot direction perference order intact.
            lChildren.Reverse();
            foreach (Cell cell in lChildren)
            {
                if (!aPaths.Any(c => (c.Data.X == cell.X) && (c.Data.Y == cell.Y)))
                //if(!aPaths.Any(c => c.Data == cell))
                {
                    // get the gScore value from the parent (current cell) - the elapsed distance.
                    fGScoreMap.TryGetValue(lCurrent, out double lIntFromDict);

                    // tentative gscore is the distance from intial cell to the neighbour through the current cell (1). 
                    double lTentativeGScore = lIntFromDict + cell.DistanceToNext;

                    CellState lNewCellState = new CellState(lCurrent, cell, fEvalFunction);

                    aPaths.Add(lNewCellState);

                    // search down the path with a successor, recursively. returns the minimum fscore down that path. 
                    double lMinimumChildFScore = recursiveSearch(aPaths, aGraphCost + lTentativeGScore, aThreshold);
                    fDiscovered++;
                    //Console.WriteLine("Min " + lMinimumIsFound);

                    // goal is found here
                    if (lMinimumChildFScore == 0.0)
                    {
                        return 0.0;
                    }

                    // if the minimum child's f value (eval func) is less than the minimum found, store it. 
                    if(lMinimumChildFScore < lMinimumIsFound)
                    {
                        lMinimumIsFound = lMinimumChildFScore;
                    }

                    fGScoreMap.Add(lNewCellState, lTentativeGScore);

                    // remove the current child and searhc the next cell.
                    aPaths.RemoveAt(aPaths.Count -1);
                }
            }

            return lMinimumIsFound;
        }

    }
}
