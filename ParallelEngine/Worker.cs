using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelEngine
{
    internal class Worker
    {
        public Worker(object locker, Barrier barrier, Random rnd, double pV, double pH)
        {
            Locker = locker;
            Barrier = barrier;
            Thread = new Thread(StartMoving) { IsBackground = true };
            Random = rnd;
            PV = pV;
            PH = pH;
        }      
  
        public Particle Particle { get; set; }
        private Thread Thread { get; set; }
        private object Locker { get; set; }
        private Barrier Barrier { get; set; }
        private Random Random { get; set; }
        private double PV { get; set; }
        private double PH { get; set; }

        private void StartMoving(object obj)
        {
            Thread.Name = String.Format("Worker number {0}", Number);
            while (true)
            {
                double numberH, numberV;
                lock (Locker)
                {
                    numberH = Random.NextDouble();
                    numberV = Random.NextDouble();
                }
                int dx = (numberV <= PV) ? -1 : 1,
                    dy = (numberH <= PH) ? -1 : 1;
                Particle.Move(dx, dy);
                Barrier.SignalAndWait();
            }
        }

        internal void Start()
        {
            Thread.Start();
        }

        public int Number { get; set; }

        internal void Stop()
        {
            Thread.Abort();
        }
    }
}
