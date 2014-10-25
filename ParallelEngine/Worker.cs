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
        public Worker(object locker, Barrier barrier, Random rnd, double pv1, double pv2, double ph1, double ph2)
        {
            Locker = locker;
            Barrier = barrier;
            Thread = new Thread(StartMoving) { IsBackground = true };
            Random = rnd;
            PV1 = pv1;
            PV2 = pv2;
            PH1 = ph1;
            PH2 = ph2;
        }      
  
        public Particle Particle { get; set; }
        private Thread Thread { get; set; }
        private object Locker { get; set; }
        private Barrier Barrier { get; set; }
        private Random Random { get; set; }
        private double PV1 { get; set; }
        private double PH1 { get; set; }
        private double PV2 { get; set; }
        private double PH2 { get; set; }

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
                int dx = (numberV <= PV1) ? -1 : (numberV <= PV2 ? 0 : 1),
                    dy = (numberH <= PH1) ? -1 : (numberH <= PH2 ? 0 : 1);
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
