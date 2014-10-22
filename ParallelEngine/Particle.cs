using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelEngine
{
    public class Particle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Map Map { get; set; }
        public void Move(int vx, int vy)
        {
            Debug.Assert(Math.Abs(vx) + Math.Abs(vy) == 1);
            int newX = X + vx,
                newY = Y + vy;
            if (Map.IsInsideMap(newX, newY))
            {
                --Map.Count[X, Y];
                X = newX;
                Y = newY;
                ++Map.Count[X, Y];
            }
            //else ignore move
        }
    }
}
