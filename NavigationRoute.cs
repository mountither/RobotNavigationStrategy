using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RobotNavigation
{
    /*
     * this class is responsible for creating the navigation environment for the Robot
     * The Cells of the map will be added to a list of that type. 
     * Empty Cells will be identified
     * Neighbouring Cells surrunding a given Call is recorded.  (AdjacentCells method found in each Cell instance)
     * 
     * Heuristic to be used: Euclidean distance, since we have access to X and Y coordinates for goal and current cell. 
     *      - distance((gX, gY), (iX, iY)) = sqrt((gX - iX)^2 + (gY - iY)^2)
     *      - if more than 1 goal, the minimum is chosen. 
     */

    class NavigationRoute
    {
        List<EmptyCellStateData> fEmptyCellStates;
        List<GoalStateData> fGoalList;
        List<Cell> fCells = new List<Cell>();

        List<Cell> fEmptyCells = new List<Cell>();

        public NavigationRoute(MapStateData aMapState, List<EmptyCellStateData> aEmptyCellStates, List<GoalStateData> aGoalList)
        {
            MapHeight = aMapState.Height;
            MapWidth = aMapState.Width;
            fGoalList = aGoalList;
            fEmptyCellStates = aEmptyCellStates;

            buildMapEnvironment();
        }

        // create the map environment
        public void buildMapEnvironment()
        {
            // render the positional cells
            renderCellsOnMap();
            
            // mark cells with isEmpty = true
            identifyEmptyCells();

            // add to seperate list of empty cells
            addToEmptyCellList();

            // for each cell in the list, add the relevant neighbour(s)
            for(int i = 0; i < fCells.Count; i++)
            {
                if (!fCells[i].IsEmpty)
                {
                    allocateNeighboursToCell(fCells[i], i);
                }
            }

            // some cells have empty cells near them. Must remove from Adjecent List
            removeAdjacentEmptyCells();
        }

        private void renderCellsOnMap()
        {
            for (int i = 0; i < MapHeight; i++)
            {
                for (int j = 0; j < MapWidth; j++)
                {

                    fCells.Add(new Cell(j, i, calculateEuclideanDist(j, i), 1));
                }
            }
        }
        private double calculateEuclideanDist(int aX, int aY)
        {
            // calculate the goal(s) in goalList and return the minimum between the two. 
            List<double> lDistanceArray = new List<double>();
            foreach (GoalStateData lGoal in fGoalList)
            {
                lDistanceArray.Add(Math.Sqrt(Math.Pow(lGoal.X - aX, 2) + Math.Pow(lGoal.Y - aY, 2)));
            }
            return Math.Round(lDistanceArray.Min(),2);
        }

        // acknowledge the empty cells. 
        private void identifyEmptyCells()
        {
            // must loop through the empty cell data list
            foreach(EmptyCellStateData lEc in fEmptyCellStates)
            {
                // loop through the x and y dimensions of given empty cell(s)
                // (x, y, height, width)
                // lEc.Width + offset (x position)

                for (int i = lEc.Y; i < (lEc.Y + lEc.Height); i++)
                {
                    for (int j = lEc.X; j < (lEc.X + lEc.Width); j++)
                    {
                        // search for cell inside cell list with same coordin. and set isEmpty
                        int lInd = fCells.FindIndex(cell => cell.X == j && cell.Y == i);
                        fCells[lInd].IsEmpty = true;
                    }
                }
            }

        }

        private void addToEmptyCellList()
        {
            foreach(Cell cell in fCells)
            {
                if (cell.IsEmpty)
                {
                    fEmptyCells.Add(cell);
                }
            }
        }

        // allocate each cell an appropraite neighbour
        private void allocateNeighboursToCell(Cell aCell, int aIndex)
        {
            bool lCanGoRight = aCell.X < MapWidth - 1;
            bool lCanGoLeft = aCell.X > 0;
            bool lCanGoUp = aCell.Y > 0;
            bool lCanGoDown = aCell.Y < MapHeight - 1;

            // the direction possibilties are known based on bools above
            // check each bool and add neighbour accordingly
            /*
                 * Must sort the Cells in a specific order for expantion.
                 * The criteria:
                 * When all else is equal nodes should:
                 *      - move Up first
                 *      - before attempting Left
                 *      - Down
                 *      - Right
                 *      
            */
            if (lCanGoUp)
            {
                aCell.AdjacentCells.Add(new AdjacentCell(fCells[aIndex - MapWidth]));
            }
            if (lCanGoLeft)
            {
                aCell.AdjacentCells.Add(new AdjacentCell(fCells[aIndex - 1]));

            }
            if (lCanGoDown)
            {
                aCell.AdjacentCells.Add(new AdjacentCell(fCells[MapWidth + aIndex]));

            }
            if (lCanGoRight)
            {
                aCell.AdjacentCells.Add(new AdjacentCell(fCells[aIndex + 1]));
            }

        }

        private void removeAdjacentEmptyCells()
        {
            foreach(Cell cell in fCells)
            {
                for(int i = 0; i < cell.AdjacentCells.Count; i++)
                {
                    if (cell.AdjacentCells[i].Data.IsEmpty)
                    {
                        cell.AdjacentCells.Remove(cell.AdjacentCells[i]);
                    }
                }
            }
        }

        public int MapWidth { get; }
        public int MapHeight { get; }

        // used by win forms (GUI).
        public List<Cell> EmptyCellList
        {
            get { return fEmptyCells; }
        }
        public List<Cell> CellList
        {
            get { return fCells; }
        }

    }
}
