using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace RobotNavigation
{
    /*   
     *   this class reads the file and the extract method is used to extract
     *   Map dimensions (X, Y)
     *   The Robot's Initial State (X, Y)
     *   The goal state (X, Y)
     *   State of empty cells (X, Y, W, H)
    */
    class FileContent
    {
        private StreamReader fFileReader;

        private MapStateData fMapDimensions;
        private InitialStateData fInitialCoord;
        private List<GoalStateData> fGoalCoordList;
        private List<EmptyCellStateData> fEmpCellList;

        public FileContent(string aFileName)
        {
            fFileReader = new StreamReader(aFileName);

            fGoalCoordList = new List<GoalStateData>();
            fEmpCellList = new List<EmptyCellStateData>();

            extract();
        }

        public void extract()
        {
            try
            {
                // read first line + regex + assign map dim/init pos.
                string lMapDimLine = fFileReader.ReadLine();
                string lMapDimClean = removeBracket(lMapDimLine);
                fMapDimensions = new MapStateData(Int32.Parse(lMapDimClean.Split(',')[1]), Int32.Parse(lMapDimClean.Split(',')[0]));

                string lInitPosLine = fFileReader.ReadLine();
                string lInitPosClean = removeParenthesis(lInitPosLine);
                fInitialCoord = new InitialStateData(Int32.Parse(lInitPosClean.Split(',')[0]), Int32.Parse(lInitPosClean.Split(',')[1]));

                // must extract one or more goal positions here, seperate by a pipe. 

                string lGoalPosLine = fFileReader.ReadLine();
                string lGoalPosClean;

                // if pipe, many goal pos exist. 
                if (lGoalPosLine.Contains("|"))
                {
                    foreach (string lGoalPos in lGoalPosLine.Split('|'))
                    {
                        lGoalPosClean = removeParenthesis(lGoalPos);

                        GoalStateData lGoalPosInstance = new GoalStateData(Int32.Parse(lGoalPosClean.Split(',')[0]), Int32.Parse(lGoalPosClean.Split(',')[1]));

                        fGoalCoordList.Add(lGoalPosInstance);
                    }

                }
                else
                {
                    lGoalPosClean = removeParenthesis(lGoalPosLine);

                    GoalStateData lGoalPosInstance = new GoalStateData(Int32.Parse(lGoalPosClean.Split(',')[0]), Int32.Parse(lGoalPosClean.Split(',')[1]));
                    fGoalCoordList.Add(lGoalPosInstance);
                }


                // read the rest of the file. Which happens to be locations of empty cells
                while (!fFileReader.EndOfStream)
                {
                    string lEmptyCellLine = fFileReader.ReadLine();

                    string lEmptyCellClean = removeParenthesis(lEmptyCellLine);
                    // initialise empty cell instance with leftmost X and Y, the width and height. + add in list. 
                    EmptyCellStateData lEmptyCellInstance = new EmptyCellStateData(Int32.Parse(lEmptyCellClean.Split(',')[0]),
                                                                            Int32.Parse(lEmptyCellClean.Split(',')[1]),
                                                                            Int32.Parse(lEmptyCellClean.Split(',')[2]),
                                                                            Int32.Parse(lEmptyCellClean.Split(',')[3]));

                    fEmpCellList.Add(lEmptyCellInstance);
                }
                

                fFileReader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private string removeParenthesis(string aDirtyLine)
        {
            return Regex.Replace(aDirtyLine, @"[\(\)']+", "");
        }
        private string removeBracket(string aDirtyLine)
        {
            return Regex.Replace(aDirtyLine, @"[\[\]']+", "");
        }

        public MapStateData MapDim
        {
            get
            {
                return fMapDimensions;
            }
        }

        public List<EmptyCellStateData> EmptyCellList
        {
            get
            {
                return fEmpCellList;
            }
        }
        public InitialStateData InitPos
        {
            get
            {
                return fInitialCoord;
            }
        }

        public List<GoalStateData> GoalCoordList
        {
            get
            {
                return fGoalCoordList;
            }
        }
    }
}
