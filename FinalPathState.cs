using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    struct FinalPathState
    {

        public FinalPathState(int aX, int aY)
        {
            X = aX;
            Y = aY;
        }
        public int X { get; }
        public int Y { get; }

    }
}
