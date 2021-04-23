﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigation
{
    struct InitialStateData
    {
        public InitialStateData(int aX, int aY)
        {
            X = aX;
            Y = aY;
        }
        public int X { get; }
        public int Y { get; }
    }
}
