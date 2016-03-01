using System;
using System.Collections.Generic;
using System.Linq;
using GrafosLib.Model;

namespace GrafosLib.Algorithms
{
    public class ColoringAlgorithm
    {
        private readonly IGraph _graph;
        private int _colorCount;

        public ColoringAlgorithm(IGraph graph)
        {
            _graph = graph;
            _colorCount = 0;
        }

        /// <summary>
        ///     Color the vertices in the graph using the Welsh-Powell algorithm
        /// </summary>
        public void RunColoring()
        {
            //Order the vertices in descending order according to their degree
            var orderedVertices = _graph.Degrees().OrderByDescending(i => i.Value).Select(a => a.Key).ToList();
            var colorMap = new int[_graph.VerticesCount + 1];

            while (orderedVertices.Any())
            {
                var colored = new List<int>();
                _colorCount++;

                foreach (var vertex in orderedVertices)
                {
                    if (colorMap[vertex] == 0 && !Conflict(vertex, colored))
                    {
                        colored.Add(vertex);
                        colorMap[vertex] = _colorCount;
                    }
                }

                foreach (var c in colored)
                {
                    orderedVertices.Remove(c);
                }
            }

            if (!IsValidColoring(colorMap))
            {
                throw new Exception("Invalid Coloring!");
            }
        }

        private bool Conflict(int vertex, List<int> colored)
        {
            var n = _graph.Neighbours(vertex).Select(e => e.Target);
            return n.Intersect(colored).Any();
        }

        public int ColorCount()
        {
            return _colorCount;
        }

        public bool IsValidColoring(int[]colorMap)
        {
            return !colorMap.Where((t, i) => _graph.Neighbours(i).Any(neighbour => colorMap[neighbour.Target] == t)).Any();
        }
    }
}