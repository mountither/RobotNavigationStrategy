using System;
using System.Collections.Generic;
using System.Text;

namespace RobotNavigation
{
    struct MapStateData
    {
        public MapStateData(int aWidth, int aHeight)
        {
            Width = aWidth;
            Height = aHeight;
        }
        public int Width { get; }
        public int Height { get; }
    }
}
