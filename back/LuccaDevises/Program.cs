namespace LuccaDevises
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var provider = new LuccasDevisesProvider();
            if (args.Length != 1)
            {
                Console.WriteLine($"Wrong number of arguments");
            }
            List<Exchange> exchanges = null;
            EntryExchangeTransaction transaction = null;
            try
            {
                var result = provider.LoadEntryFile(args[0]);
                transaction = result.Item1;
                exchanges = result.Item2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Dictionary<string, List<string>> adjacencyList = new Dictionary<string, List<string>>();
            foreach (var exchange in exchanges)
            {
                if (!adjacencyList.Keys.Contains(exchange.Start))
                {
                    adjacencyList.Add(exchange.Start, new List<string> { exchange.Target });
                }
                if (!adjacencyList.Keys.Contains(exchange.Target))
                {
                    adjacencyList.Add(exchange.Target, new List<string> { exchange.Start });
                }
                if (adjacencyList.Keys.Contains(exchange.Start) && !adjacencyList[exchange.Start].Contains(exchange.Target))
                {
                    adjacencyList[exchange.Start].Add(exchange.Target);
                }
                if (adjacencyList.Keys.Contains(exchange.Target) && !adjacencyList[exchange.Target].Contains(exchange.Start))
                {
                    adjacencyList[exchange.Target].Add(exchange.Start);
                }
            }

            var startingNode = transaction.DeviseTargeted;
            List<string> nodesToVisit = new List<string>
            {
                startingNode,
            };
            List<string> nodesAlreadyVisited = new List<string>();
            while(nodesToVisit.Count > 0)
            {
                var startingPoint = nodesToVisit.First();
                nodesAlreadyVisited.Add(startingPoint);
                var adjacentNodes = adjacencyList[startingPoint];
                foreach(var node in adjacentNodes)
                {
                    if (!nodesToVisit.Contains(node) && !nodesAlreadyVisited.Contains(node))
                    {
                        nodesToVisit.Add(node);
                    }
                }
                nodesToVisit.RemoveAt(0);
            }
        }
    }
}
