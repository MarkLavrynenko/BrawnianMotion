using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
            Debug.Assert(Math.Abs(vx) + Math.Abs(vy) <= 2);
            int newX = X + vx,
                newY = Y + vy;
            if (Map.IsInsideMap(newX, newY))
            {
                int oldVal, newVal;
                do {
                    oldVal = Map.Count[X,Y];
                    newVal = oldVal - 1;
                } while (Interlocked.CompareExchange(ref Map.Count[X,Y], newVal, oldVal) != oldVal);

                do {
                    oldVal = Map.Count [newX, newY];
                    newVal = oldVal + 1;
                } while (Interlocked.CompareExchange(ref Map.Count[newX, newY], newVal, oldVal) != oldVal);
                X = newX;
                Y = newY;
            }
            //else ignore move
        }
    }
}
