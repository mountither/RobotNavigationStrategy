using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotNavigation
{
    /*
     * This class takes in the Goal State(s)
     * it checks whether the goal(s) coordinates have been met (method)
     * can output the result to the Console using the directions the optimal search took (traversed)   
     * This class will be instantiated by each search strategy. 
     */
    class NavigationPlan
    {
        private List<GoalStateData> fGoalState;
        private List<FinalPathState> fFinalCoordinateList;

        // nav route may be redundent
        public NavigationPlan(List<GoalStateData> aGoalState)
        {
            fGoalState = aGoalState;
            fFinalCoordinateList = new List<FinalPathState>();
        }

        /*  check whether the goal has been reached. consider multiple goals can be reached.
            this method will be likely used by the search strat. a parameter of the current state
            will be sent here. Must check if the current state satisfy the goal(s)
        */
        public bool isSolved(CellState aCell)
        {
            // list of Goal state may have one item in list. 

            if(fGoalState.Count > 1)
            {
                foreach(GoalStateData lGoal in fGoalState)
                {
                    if (lGoal.X == aCell.Data.X && lGoal.Y == aCell.Data.Y)
                    {
                        return true;
                    }
                }
            }
            return (fGoalState[0].X == aCell.Data.X && fGoalState[0].Y == aCell.Data.Y);
        
        }

        public List<Cell> getMoveSet(CellState aState)
        {

            List<Cell> lCells = new List<Cell>();

            foreach (AdjacentCell cell in aState.Data.AdjacentCells)
            {
                lCells.Add(cell.Data);
            }

            return lCells;
        }

        public void outputResult(CellState aState, int aSearch, int aDiscover)
        {
            //Create a stack that will have each node in the 
            //solution chain added until we reach the starting node in the search
            //stack is a special type of collection that stores elem in LIFO style. 

            Stack<string> lStack = new Stack<string>();

            if(aState == null)
            {
                Console.WriteLine("No Solutions Found");
            }

            // check the parent position relative to the current state + recurse aState (traverse)

            while (aState != null && aState.ParentCell != null)
            {
                if(aState.ParentCell.Data.X == aState.Data.X + 1)
                {
                    lStack.Push("Left" + " to (" + aState.Data.X + "," + aState.Data.Y + ")");
                }
                if (aState.ParentCell.Data.X == aState.Data.X - 1)
                {
                    lStack.Push("Right" + " to (" + aState.Data.X + "," + aState.Data.Y + ")");
                }
                if (aState.ParentCell.Data.Y == aState.Data.Y + 1)
                {
                    lStack.Push("Up" + " to (" + aState.Data.X + "," + aState.Data.Y + ")");
                }
                if (aState.ParentCell.Data.Y == aState.Data.Y - 1)
                {
                    lStack.Push("Down" + " to (" + aState.Data.X + "," + aState.Data.Y + ")");
                }

                fFinalCoordinateList.Add(new FinalPathState(aState.Data.X, aState.Data.Y));

                aState = aState.ParentCell;

            }

            Console.WriteLine("searched: " + aSearch + " discovered: " + aDiscover);

            while (!(lStack.Count == 0))
            {
                Console.WriteLine(lStack.Pop());
            }
        }

        // used by win form (GUI)
        public List<FinalPathState> FinalCoordinateList
        {
            get
            {
                return fFinalCoordinateList;
            }
        }



    }

}


