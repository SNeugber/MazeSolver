using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Priority_Queue;

namespace MazeSolver
{
    class Node : PriorityQueueNode
    {
        public Node previous; // { public get; public set; }
        public int distance; // { public get; public set; }
        public bool visited;//  { get; set; }
        public Point position { get; private set; }
        public bool traverseable { get; private set; }

        public Node(Point position, bool traverseable)
        {
            this.position = position;
            this.traverseable = traverseable;
            this.distance = int.MaxValue;
            this.visited = false;
            // previous set as invalid to begin with, i.e. remains null
        }
    }
}
