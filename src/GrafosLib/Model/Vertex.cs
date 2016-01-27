using System.Collections.Generic;

namespace GrafosLib.Model
{
    public class Vertex
    {
        public int Value { get; set; }
        public List<Edge> Edges { get; set; }
        public int Degree { get; set; } 
    }
}