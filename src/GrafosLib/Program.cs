using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                Console.WriteLine("Choose file");
                var file = Console.ReadLine();
                //TextReader fin = new StreamReader($"c:\\Grafos\\grafo_{file}.txt");
                TextReader fin = new StreamReader($"c:\\Grafos\\rede_colaboracao.txt");
                Console.WriteLine($"Reading file {file}...");
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

            var sw = new Stopwatch();

            sw.Start();

            // run dijkstra
            //Console.WriteLine("Run Dijkstra");

            //var d = new Dijkstra(g);
            //d.Run(10);

            //sw.Stop();
            //Console.WriteLine("End");
            //Console.WriteLine($"Distance to vertex 1: {d.DistanceTo(1)}");

            //Console.WriteLine("Run Prim");
            //var p = new Prim(g);
            //sw.Stop();

            //Console.WriteLine($"MST weight: {p.Weight()}");

            //var d = new Dijkstra(g);
            //d.Run(2722);

            //sw.Stop();
            //Console.WriteLine("End");
            //Console.WriteLine($"Distance to turing: {d.DistanceTo(11365)}");
            //Console.WriteLine($"Distance to kruskal: {d.DistanceTo(211706)}");
            //Console.WriteLine($"Distance to kleinberg: {d.DistanceTo(264337)}");
            //Console.WriteLine($"Distance to tardos: {d.DistanceTo(11386)}");
            //Console.WriteLine($"Distance to daniel: {d.DistanceTo(343930)}");

            //var pathToDaniel = d.PathTo(343930);
            //foreach (var edge in pathToDaniel)
            //{
            //    Console.WriteLine(edge);
            //}

            Console.WriteLine("Run Prim");
            var p = new Prim(g);
            sw.Stop();

            Console.WriteLine($"MST weight: {p.Weight()}");
            var mstEdges = p.Edges();
            var mst = new AdjacencyListGraph(true);
            foreach (var mstEdge in mstEdges)
            {
                mst.AddEdge(mstEdge.Source, mstEdge.Target, mstEdge.Weight);
            }
            var degrees = mst.Degrees();
            var d = degrees.Values.OrderByDescending(a => a).ToList();
            Console.WriteLine($"Degree 1: {d[0]}");
            Console.WriteLine($"Degree 2: {d[1]}");
            Console.WriteLine($"Degree 3: {d[2]}");

            Console.WriteLine(
                $"Dijkstra neighbours: {mst.Neighbours(2722).Aggregate("", (current, neighbour) => current + (neighbour + " - "))}");
            Console.WriteLine(
                $"Figueiredo neighbours: {mst.Neighbours(343930).Aggregate("", (current, neighbour) => current + (neighbour + " - "))}");



            Console.WriteLine("Elapsed={0}", sw.Elapsed.Milliseconds);
            Console.ReadLine();
        }
    }
}