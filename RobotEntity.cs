using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotNavigation
{
    /*
     * This class is responsible for triggering a search strategy based on the user input
     * A method will be specified for each available Search strat. 
     * the method will instantiate the relevant strategy class.  
     * This entity will take in a Navigation Plan for methods to utilise
     */
    class RobotEntity
    {
        private NavigationPlan fNavPlan;
        private NavigationRoute fNavRoute;
        private InitialStateData fInitPos;
        public RobotEntity(NavigationPlan aNavPlan, NavigationRoute aNavRoute, InitialStateData aInitPos)
        {
            fNavPlan = aNavPlan;
            fNavRoute = aNavRoute;
            fInitPos = aInitPos;
        }

        // breadth first search - uninformed
        public void executeBFS()
        {
            BFS lBfs = new BFS(fNavPlan);

            lBfs.search(new CellState(null, 
                fNavRoute.CellList.FirstOrDefault(cell => cell.X == fInitPos.X && cell.Y == fInitPos.Y)));

        }

        // deep first search - uninformed
        public void executeDFS()
        {
            DFS lDfs = new DFS(fNavPlan);
            lDfs.search(new CellState(null,
                fNavRoute.CellList.FirstOrDefault(cell => cell.X == fInitPos.X && cell.Y == fInitPos.Y)));
        }

        // deep limited search - uninformed
        public void executeDLS()
        {
            // Must specify an appropriate depth limit. This will depend on factors such as state space (map dim). 
            int lDepthLimit = 34;

            DLS lAstar = new DLS(fNavPlan);
            lAstar.search(new CellState(null,
                fNavRoute.CellList.FirstOrDefault(cell => cell.X == fInitPos.X && cell.Y == fInitPos.Y)), lDepthLimit);
        }

        // greedy best first search - informed
        public void executeGBFS()
        {
            GBFS lGbfs = new GBFS(fNavPlan);
            lGbfs.search(new CellState(null,
                fNavRoute.CellList.FirstOrDefault(cell => cell.X == fInitPos.X && cell.Y == fInitPos.Y)));
        }

        // Astar - uninformed
        public void executeASTAR()
        {
            ASTAR lAstar = new ASTAR(fNavPlan);
            lAstar.search(new CellState(null,
                fNavRoute.CellList.FirstOrDefault(cell => cell.X == fInitPos.X && cell.Y == fInitPos.Y)));
        }

        // Iterative-deepening Astar - informed
        public void executeIDA()
        {
            IDA lAstar = new IDA(fNavPlan);
            lAstar.search(new CellState(null,
                fNavRoute.CellList.FirstOrDefault(cell => cell.X == fInitPos.X && cell.Y == fInitPos.Y)));
        }

    }
}
