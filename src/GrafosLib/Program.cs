using System;
using System.Diagnostics;
using System.Globalization;
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
                TextReader fin = new StreamReader($"c:\\Grafos\\G{file}.txt");
                //TextReader fin = new StreamReader($"c:\\Grafos\\rede_colaboracao.txt");
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
                            if (st.Length == 3)
                            {
                                var source = int.Parse(st[0]);
                                var dest = int.Parse(st[1]);
                                var culture = new CultureInfo("en");
                                var cost = double.Parse(st[2], culture);
                                g.AddEdge(source, dest, cost);

                            }else if (st.Length == 2)
                            {
                                var source = int.Parse(st[0]);
                                var dest = int.Parse(st[1]);
                                g.AddEdge(source, dest);
                            }
                            else
                            {
                                Console.Error.WriteLine("Skipping ill-formatted line " + line);
                            }
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
            Console.WriteLine(g.Edges().Count()/2 + " edges");

            var sw = new Stopwatch();

            sw.Start();

            var coloring = new ColoringAlgorithm(g);

            coloring.RunColoring();

            Console.WriteLine("Colors={0}", coloring.ColorCount());
            Console.WriteLine("Elapsed={0}", sw.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }
    }
}