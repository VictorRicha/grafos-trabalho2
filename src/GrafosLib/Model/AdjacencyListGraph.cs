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
            var e = new Edge(target, source, cost);
            _vertices.Add(source, e);

            if (!IsDirected)
                _vertices.Add(target, e);
        }
    }
}