using System;

namespace GrafosLib.Model
{
    public class Edge : IComparable<Edge>
    {
        public Edge(int target, int source, double weight)
        {
            Source = source;
            Target = target;
            Weight = weight;
        }

        public int Source { get; set; }
        public int Target { get; set; }
        public double Weight { get; set; }

        /// <summary>
        /// Compara duas arestas
        /// </summary>
        public int CompareTo(Edge other)
        {
            if (Weight < other.Weight) return -1;
            if (Weight > other.Weight) return 1;
            return 0;
        }

        public override string ToString()
        {
            return Source + " -> " + Target;
        }
    }
}