namespace GrafosLib.Algorithms {
    using System;
    using System.Linq;
    using Model;

    public class AveragePathLength {

        private readonly IGraph _graph;
        private double _distanceSum;

        public AveragePathLength(IGraph graph) {
            _graph = graph;
            _distanceSum = 0;
            
        }

        public double Run() {
            var vertices = _graph.Vertices();
            var enumerable = vertices as int[] ?? vertices.ToArray();
            foreach (var vertex in enumerable) {
                var dijkstra = new Dijkstra(_graph);
                dijkstra.Run(vertex);

                foreach (var v in enumerable) {
                    if (v != vertex)
                        if(!double.IsPositiveInfinity(dijkstra.DistanceTo(v)))
                            _distanceSum += dijkstra.DistanceTo(v);
                }
            }

            return _distanceSum/(_graph.VerticesCount*(_graph.VerticesCount - 1));
        } 
    }
}