using System.Collections.Generic;
using System.Linq;
using GrafosLib.Model;

namespace GrafosLib.Algorithms
{
    public class BreadthFirstSearchAlgorithm
    {
        private readonly IGraph _graph;
        private bool[] _visited;

        public BreadthFirstSearchAlgorithm(IGraph graph, int[] parents)
        {
            _graph = graph;
        }

        public void RunBFS(int rootVertex)
        {
            _visited = new bool[_graph.VerticesCount + 1];

            for (var i = 0; i < _graph.VerticesCount; i++)
                _visited[i] = false;

            var queue = new Queue<int>();

            _visited[rootVertex] = true;
            queue.Enqueue(rootVertex);

            while (queue.Any())
            {
                rootVertex = queue.Dequeue();
                foreach (var neighbour in _graph.Neighbours(rootVertex))
                {
                    if (!_visited[neighbour.Target])
                    {
                        _visited[neighbour.Target] = true;
                        queue.Enqueue(neighbour.Target);
                    }
                }
            }
        }
    }
}