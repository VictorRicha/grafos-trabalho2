using System.Collections.Generic;
using System.Linq;
using GrafosLib.DataStructure;

namespace GrafosLib.Model
{
    public class AdjacencyListGraph : IGraph
    {
        public bool IsDirected { get; set; }
        public bool IsWeighted { get; set; }
        public int VerticesCount { get; set; }
        private readonly DictionaryOfCollection<int, Edge> _vertices;

        public AdjacencyListGraph(bool isWeighted = false, bool isDirected = false)
        {
            _vertices = new DictionaryOfCollection<int, Edge>();
            IsDirected = isDirected;
            IsWeighted = isWeighted;
        }

        public void AddEdge(int source, int target, double cost = 1)
        {
            _vertices.Add(source, new Edge(target, source, cost));

            if (!IsDirected)
                _vertices.Add(target, new Edge(source, target, cost));
        }

        public IEnumerable<Edge> Edges()
        {
            return _vertices.Values;
        }

        public IEnumerable<Edge> Neighbours(int vertex)
        {
            return _vertices[vertex];
        }

        public IEnumerable<int> Vertices()
        {
            return _vertices.Keys;
        }

        public IDictionary<int, int> Degrees()
        {
            return _vertices.ToDictionary(vertex => vertex.Key, vertex => vertex.Value.Count());
        }
    }
}