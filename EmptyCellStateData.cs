using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigation
{ 
    struct EmptyCellStateData
    {
        public EmptyCellStateData(int aLeftMostX, int aLeftMostY, int aWidth, int aHeight)
        {
            X = aLeftMostX;
            Y = aLeftMostY;
            Height = aHeight;
            Width = aWidth;
        }
        public EmptyCellStateData(int aX, int aY)
        {
            X = aX;
            Y = aY;
            Width = 1;
            Height = 1;
        }
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

    }
}
