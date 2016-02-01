using System;
using System.IO;
using GrafosLib.Algorithms;
using GrafosLib.Model;

namespace GrafosLib
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var g = new AdjacencyListGraph(true);
            try
            {
                //     TextReader fin = new StreamReader( args[0] );
                TextReader fin = new StreamReader("c:\\Grafos\\tinyEWD.txt");

                var counter = 1;
                string line;
                while ((line = fin.ReadLine()) != null)
                {
                    var st = line.Split();

                    try
                    {
                        if (counter == 1)
                        {
                            g.VerticesCount = int.Parse(line);
                        }
                        else
                        {
                            if (st.Length != 3)
                            {
                                Console.Error.WriteLine("Skipping ill-formatted line " + line);
                                continue;
                            }
                            var source = int.Parse(st[0]);
                            var dest = int.Parse(st[1]);
                            var cost = double.Parse(st[2]);
                            g.AddEdge(source, dest, cost);
                        }
                    }
                    catch (FormatException) { Console.Error.WriteLine("Skipping ill-formatted line " + line); }
                    finally { counter++;}
                }
            }
            catch (IOException e)
            { Console.Error.WriteLine(e); }

            Console.WriteLine("File read...");
            Console.WriteLine(g.VerticesCount + " vertices");

            // run dijkstra
            Console.WriteLine("Run Dijkstra");
            var d = new Dijkstra(g);
            d.Run(0);
            Console.WriteLine("End");
            foreach (var vertex in g.Vertices())
            {
                if(!double.IsPositiveInfinity(d.DistanceTo(vertex)))
                    Console.WriteLine($"distance to vertex {vertex} is {d.DistanceTo(vertex)}");
            }

            Console.WriteLine("path to vertex 1:");
            var path = d.PathTo(6);
            if (path == null)
            {
                Console.WriteLine("there's no path");
            }
            else
            {
                foreach (var edge in path)
                {
                    Console.WriteLine(edge);
                }
            }

            Console.WriteLine("Run Prim");
            var p = new Prim(g);

            Console.WriteLine($"MST weight: {p.Weight()}");

            var mst = p.Edges();

            Console.WriteLine("MST:");
            foreach (var edge in mst)
            {
                Console.WriteLine(edge);
            }

            Console.ReadLine();
        }
    }
}