using System.Collections;
using System.Collections.Generic;

namespace GrafosLib.Model
{
    public interface IGraph
    {
        void AddEdge(int source, int target, double cost);
        bool IsDirected { get; }
        bool IsWeighted { get; }
        int VerticesCount { get; }
        IEnumerable<Edge> Edges();
        IEnumerable<Edge> Neighbours(int vertex);
        IEnumerable<int> Vertices();
        IDictionary<int, int> Degrees();
    }
}