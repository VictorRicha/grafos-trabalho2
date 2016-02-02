using System.Linq;

namespace GrafosLib.Algorithms
{
    using System.Collections.Generic;
    using Model;

    public enum VertexState { White, Gray, Black };
    public class DepthFirstSearchAlgorithm
    {
        private readonly IGraph _graph;

        public DepthFirstSearchAlgorithm(IGraph graph)
        {
            _graph = graph;
        }

        public void RunDfs(int rootVertex)
        {
            var stack = new Stack<int>();
            var visited = new HashSet<int>();

            stack.Push(rootVertex);

            while (stack.Count != 0)
            {
                var current = stack.Pop();
                if (!visited.Add(current))
                    continue;

                var neighbours = _graph.Neighbours(current);
                foreach (var neighbour in neighbours.Where(n => !visited.Contains(n.Target)))
                {
                    stack.Push(neighbour.Target);
                }
            }
        }
    }
}