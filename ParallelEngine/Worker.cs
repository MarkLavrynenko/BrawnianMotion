using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelEngine
{
    public class Worker
    {
        public Worker(object locker, Barrier barrier, Random rnd, Particle particle, double pV, double pH)
        {
            Locker = locker;
            Barrier = barrier;
            Thread = new Thread(StartMoving);
            Random = rnd;
            Particle = particle;
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
                lock (Locker)
                {
                    double numberH = Random.NextDouble(),
                           numberV = Random.NextDouble();
                    if (numberV <= PV)
                        Particle.Move(-1, 0);
                    else
                        Particle.Move(+1, 0);

                    if (numberH <= PH)
                        Particle.Move(0, -1);
                    else
                        Particle.Move(0, +1);
                }
               Barrier.SignalAndWait();
            }
        }

        internal void Stop()
        {
            throw new NotImplementedException();
        }

        internal void Start()
        {
            Thread.Start();
        }

        public int Number { get; set; }
    }
}
