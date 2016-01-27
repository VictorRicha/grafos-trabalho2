using System;
using System.IO;
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
                TextReader fin = new StreamReader("c:\\Grafos\\grafo_5.txt");

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

            Console.Error.WriteLine("File read...");
            Console.Error.WriteLine(g.VerticesCount + " vertices");
            Console.In.ReadLine();
        }
    }
}