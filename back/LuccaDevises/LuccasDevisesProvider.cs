namespace LuccaDevises
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Transactions;

    public class LuccaDevises : ILuccasDevisesProvider
    {
        public (Transaction, List<Conversion>) LoadEntryFile(string filename)
        {
            List<Conversion> devises = new List<Conversion>();

            if ( (!File.Exists(filename)))
            {
                Console.WriteLine("Cannot load the file content. File does not exists or the path is not the right one !");
                return (null, null);
            }
            var data = File.ReadAllLines(filename, encoding: Encoding.UTF8).ToList();
            if (data.Count < 2)
            {
                Console.WriteLine("Cannot convert this value. Not enough data");
                return (null, null);

            }
            var entryDevise = new Transaction(data[0]);
            data.RemoveAt(0);
            int numberOfDevises = 0;
            var isParsingSucceed = int.TryParse(data[0], out numberOfDevises);
            if (!isParsingSucceed || numberOfDevises < 1)
            {
                Console.WriteLine("Cannot convert this value. Not enough conversion meter");
                return (null, null);

            }
            data.RemoveAt(0);
            if (data.Count < numberOfDevises)
            {
                Console.WriteLine("Missing currency conversion");
                return (null, null);
            }
            bool isDeviseStartExistsInConversionTable = false;
            bool isDeviseEndExistsInConversionTable = false;
            foreach (var currencyConversion in data)
            {
                var Exchange = new Conversion(currencyConversion);
                if (Exchange.DeviseDepart == entryDevise.DeviseDepart || Exchange.DeviseArrivee == entryDevise.DeviseDepart)
                {
                    isDeviseStartExistsInConversionTable = true;
                }
                if (Exchange.DeviseDepart == entryDevise.DeviseCible || Exchange.DeviseArrivee == entryDevise.DeviseCible)
                {
                    isDeviseEndExistsInConversionTable = true;
                }
                devises.Add(Exchange);
            }
            if (!isDeviseEndExistsInConversionTable || !isDeviseStartExistsInConversionTable)
            {
                Console.WriteLine($"There is no record allowing the conversion from {entryDevise.DeviseDepart} to {entryDevise.DeviseCible}");
                return (null, null);
            }
            return (entryDevise, devises);
        }

        public Dictionary<string, List<string>> BuildGraph(List<Conversion> exchanges)
        {
            Dictionary<string, List<string>> adjacencyList = new Dictionary<string, List<string>>();
            foreach (var exchange in exchanges)
            {
                if (!adjacencyList.Keys.Contains(exchange.DeviseDepart))
                {
                    adjacencyList.Add(exchange.DeviseDepart, new List<string> { exchange.DeviseArrivee });
                }
                if (!adjacencyList.Keys.Contains(exchange.DeviseArrivee))
                {
                    adjacencyList.Add(exchange.DeviseArrivee, new List<string> { exchange.DeviseDepart });
                }
                if (adjacencyList.Keys.Contains(exchange.DeviseDepart) && !adjacencyList[exchange.DeviseDepart].Contains(exchange.DeviseArrivee))
                {
                    adjacencyList[exchange.DeviseDepart].Add(exchange.DeviseArrivee);
                }
                if (adjacencyList.Keys.Contains(exchange.DeviseArrivee) && !adjacencyList[exchange.DeviseArrivee].Contains(exchange.DeviseDepart))
                {
                    adjacencyList[exchange.DeviseArrivee].Add(exchange.DeviseDepart);
                }
            }
            return adjacencyList;
        }

        public List<string> GetShortestPath(Dictionary<string, List<string>> adjacencyList, string startNode, string targetNode)
        {
            List<string> pathPerNode = new List<string>();
            foreach (var node in adjacencyList.Keys)
            {
                Queue<string> nodesToVisit = new Queue<string>();
                HashSet<string> nodesAlreadyVisited = new HashSet<string>();
                var path = new List<string>
                {
                    node
                };
                var paths = new List<List<string>>();
                nodesToVisit.Enqueue(node);
                while (nodesToVisit.Count > 0)
                {
                    var currentNode = nodesToVisit.Dequeue();
                    var adjacentNodes = adjacencyList[currentNode];
                    var nodesNotVisited = adjacentNodes.Where(node => !nodesToVisit.Contains(node) && !nodesAlreadyVisited.Contains(node));
                    var isDeadEnd = nodesAlreadyVisited.Count == adjacentNodes.Count;
                    nodesAlreadyVisited.Add(currentNode);
                    if (adjacentNodes.Contains(startNode))
                    {
                        path.Add(startNode);
                        pathPerNode.Add(string.Join('-', path));
                    }
                    else
                    {
                        foreach (var nodeNotVisited in nodesNotVisited)
                        {
                            path.Add(nodeNotVisited);
                            nodesToVisit.Enqueue(nodeNotVisited);
                        }
                        if (isDeadEnd || currentNode == startNode)
                        {
                            path.Add(currentNode);
                            pathPerNode.Add(string.Join('-', path));
                        }
                    }
                }
            }
            return null;
        }
        public float GetTransactionConverted(List<string> path, Dictionary<string, List<string>>, float montant)
        {
            return 0;
        }
    }
}
