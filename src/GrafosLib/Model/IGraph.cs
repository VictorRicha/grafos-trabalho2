namespace GrafosLib.Model
{
    public interface IGraph
    {
        void AddEdge(int source, int target, double cost);
        bool IsDirected { get; }
        bool IsWeighted { get; }
        int VerticesCount { get; }
    }
}