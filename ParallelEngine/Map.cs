using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelEngine
{
    public class Map : ICloneable
    {
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Count = new int[Width,Height];
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public int NumberOfParticles
        {
            get
            {
                return Width * Height;
            }
        }
        public object Locker { get; set; }
        public int[,] Count { get; set; }

        public void AddParticle(Particle particle){
            particle.Map = this;
            Count[particle.X,particle.Y]++;
        }

        //not thread-safe, called only in PostPhraseAction
        internal int Validate(int expected)
        {
            int sum = 0;
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    sum += Count[i, j];
            Debug.Assert(sum == expected);            
            return sum;
        }

        internal bool IsInsideMap(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        internal void PrintItself()
        {
            Console.Clear();
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height; ++j)
                    Console.Write("{0,2} ", Count[i,j]);
                Console.WriteLine();
            }
        }

        public object Clone()
        {
            var map = new Map(Width, Height);
            map.Count = (int[,])Count.Clone();
            return map;
        }
    }
}
