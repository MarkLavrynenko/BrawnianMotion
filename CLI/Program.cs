using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParallelEngine;

namespace CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new Manager(10, 10, 100, 2500);
            manager.Start();
        }
    }
}
