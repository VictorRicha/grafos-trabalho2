using System;
using System.Collections.Generic;
using GrafosLib.DataStructure;
using GrafosLib.Model;

namespace GrafosLib.Algorithms
{
    public class Dijkstra
    {
        /// <summary>
        ///     Array para manter as distâncias desde o vértice inicial
        /// </summary>
        private readonly double[] _distanceTo;

        private readonly Edge[] _edgeTo;

        private readonly MinPriorityQueue<double> _priorityQueue;

        private readonly IGraph _graph;

        public Dijkstra(IGraph graph)
        {
            // verifica peso negativo
            foreach (var edge in graph.Edges())
            {
                if (edge.Weight < 0)
                    throw new Exception($"edge {edge} has negative weight");
            }

            // inicializa os arrays
            _distanceTo = new double[graph.VerticesCount + 1];
            _edgeTo = new Edge[graph.VerticesCount + 1];
            for (var i = 0; i < graph.VerticesCount; i++)
            {
                _distanceTo[i] = double.PositiveInfinity;
            }
            _priorityQueue = new MinPriorityQueue<double>(graph.VerticesCount);

            _graph = graph;
        }

        public void Run(int vertex)
        {
            _distanceTo[vertex] = 0.0;
            _priorityQueue.Insert(vertex, _distanceTo[vertex]);

            while (!_priorityQueue.IsEmpty())
            {
                var v = _priorityQueue.DeleteMin();
                foreach (var neighbour in _graph.Neighbours(v))
                {
                    Relax(neighbour);
                }
            }
        }

        private void Relax(Edge edge)
        {
            //get the source and target vertex of the edge
            int v = edge.Source, w = edge.Target;
            /*
            * _distTo[w] contains the shortest path so far to the 
            * vertex w so we can compare it to the weight of of going through
            * v, if it is less use the new path instead
            * */
            if (_distanceTo[w] > _distanceTo[v] + edge.Weight)
            {
                //set the distance to w to the new(lower) weight
                _distanceTo[w] = _distanceTo[v] + edge.Weight;
                //add the edge to the list of edges in our shortest paths
                _edgeTo[w] = edge;
                if (_priorityQueue.Contains(w))
                    /*
                    * if w is already in the priority que update its minimum path and re-order the que
                    * */
                    _priorityQueue.DecreaseKey(w, _distanceTo[w]);
                else
                    _priorityQueue.Insert(w, _distanceTo[w]);
            }
        }

        public double DistanceTo(int vertex)
        {
            return _distanceTo[vertex];
        }

        public bool HasPathTo(int v)
        {
            return _distanceTo[v] < double.PositiveInfinity;
        }

        public IEnumerable<Edge> PathTo(int v)
        {
            if (!HasPathTo(v)) return null;
            var path = new Stack<Edge>();
            for (var e = _edgeTo[v]; e != null; e = _edgeTo[e.Source])
            {
                path.Push(e);
            }
            return path;
        }
    }
}