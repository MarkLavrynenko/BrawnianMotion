using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelEngine
{
    public class MapChangedEventArgs : EventArgs
    {
        public MapChangedEventArgs(Map map)
        {
            Map = map;
        }
        /// <summary>
        /// Clone of the map
        /// </summary>
        public Map Map { get; set; }
    }
}
