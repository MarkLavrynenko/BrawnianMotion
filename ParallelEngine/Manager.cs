﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelEngine
{
    public class Manager
    {
        private object _locker = new object();
        private int _moveCount;
        private int _threads;
        private Timer _timer;
        private DateTime _startTime;
        private int _drawInterval;
        
        private volatile Boolean _needPrint;
        public Manager(int width, int height, int threads, int drawInterval)
        {
            _startTime = DateTime.Now;
            _drawInterval = drawInterval;
            Map = new Map(width, height);
            Randomizer = new Random(69);
            Barrier = new Barrier(threads, AfterStep);
            Workers = new List<Worker>();
            _threads = threads;
            for (int i = 0; i < threads; ++i)
            {
                var particle = new Particle { X = 0, Y = 0 };
                Map.AddParticle(particle);
                //TODO: move props to config
                var worker = new Worker(_locker, Barrier, Randomizer, particle, 0.5, 0.5) { Number = i } ; 
                Workers.Add(worker);
            }
        }

        private void AfterStep(Barrier obj)
        {
            ++_moveCount;
            //Console.WriteLine("Finished move #" + _moveCount);
            var sum = Map.Validate(_locker, _threads);
            if (_needPrint)
            {
                _needPrint = false;
                Map.PrintItself();
                Console.WriteLine("Sum is {0}, Step is {1} in {2} seconds",sum, _moveCount, (DateTime.Now - _startTime).Seconds);
            }            
        }
        public Map Map { get; set; }
        public List<Worker> Workers { get; set; }
        private Barrier Barrier { get; set; }
        private Random Randomizer { get; set; }

        public void Start()
        {
            //TODO: do we need _locker in timer callback???
            _timer = new Timer(Tick, _locker, 0, _drawInterval);
            for (int i = 0; i < Workers.Count; ++i)
                Workers[i].Start();            
        }

        private void Tick(object state)
        {
            _needPrint = true;
            //Console.WriteLine("Tick");
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
            for (int i = 0; i < Workers.Count; ++i)
                Workers[i].Stop();
        }
    }
}
