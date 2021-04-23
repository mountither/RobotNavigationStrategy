using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigation
{
    /*
     * This class defines a neighbouring cell for a specific. A list of this type resides in that cell
     * class. When defining this, an existing Cell will be structured based on data here. 
     */
    class AdjacentCell
    {
        Cell fData;
        public AdjacentCell(Cell aData)
        {
            fData = aData;
        }
        public Cell Data
        {
            get
            {
                return fData;
            }
            set
            {
                fData = value;
            }
        }

    }
}
