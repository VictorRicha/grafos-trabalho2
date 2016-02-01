using System.Collections.Generic;
using GrafosLib.DataStructure;
using GrafosLib.Model;

namespace GrafosLib.Algorithms
{
    public class Prim
    {
        private readonly Edge[] _edgeTo; //Keep track of the edges in our minimum spanning tree      
        private readonly double[] _distTo; //Keep track of the weights to each edge in our minimum spanning tree   
        private readonly bool[] _marked; //Keep track of which vertex we've looked

        private readonly MinPriorityQueue<double> _pq;


        public Prim(AdjacencyListGraph g)
        {
            //initialize the various arrays and the minimum priority queue
            _edgeTo = new Edge[g.VerticesCount+1];
            _distTo = new double[g.VerticesCount+1];
            _marked = new bool[g.VerticesCount+1];
            _pq = new MinPriorityQueue<double>(g.VerticesCount);
            //for each edge in the minimum spanning tree set the weight equal to infinity
            for (var v = 0; v < g.VerticesCount; v++) _distTo[v] = double.PositiveInfinity;

            for (var v = 0; v < g.VerticesCount; v++)
                if (!_marked[v]) Run(g, v);
        }

        private void Run(IGraph g, int s)
        {
            //set the weight to source vertex s as 0
            _distTo[s] = 0.0;
            //insert the vertex into the priority queue as (vertex number, weight)
            _pq.Insert(s, _distTo[s]);
            while (!_pq.IsEmpty())
            {
                //remove a vertex from the top of the queue
                var v = _pq.DeleteMin();
                Scan(g, v);
            }
        }

        private void Scan(IGraph g, int v)
        {
            _marked[v] = true;

            foreach (var e in g.Neighbours(v))
            {
                var w = e.Target;
                if (_marked[w]) continue;
                if (e.Weight < _distTo[w])
                {
                    _distTo[w] = e.Weight;
                    _edgeTo[w] = e;
                    if (_pq.Contains(w)) _pq.ChangeKey(w, _distTo[w]);
                    else _pq.Insert(w, _distTo[w]);
                }
            }
        }

        //Return all the edges in the MST
        public IEnumerable<Edge> Edges()
        {
            var mst = new Queue<Edge>();
            for (var v = 0; v < _edgeTo.Length; v++)
            {
                var e = _edgeTo[v];
                if (e != null)
                {
                    mst.Enqueue(e);
                }
            }
            return mst;
        }

        /*
        * Return the total weight of the of the minimum spanning tree.  The weight
        * should be no larger than the weight of any other spanning tree.
        * */

        public double Weight()
        {
            var weight = 0.0;
            foreach (var e in Edges())
                weight += e.Weight;
            return weight;
        }
    }
}