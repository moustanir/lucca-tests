namespace LuccaDevises
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class LuccaDevisesHelper
    {
        public static Entree LireEntree(string[] args)
        {
            var filename = args[0];
            var data = File.ReadAllLines(filename, encoding: Encoding.UTF8).ToList();
            string[] firstLine = data[0].Split(';');
            string deviseInitiale = firstLine[0];
            int montant = int.Parse(firstLine[1]);
            string deviseCible = firstLine[2];
            int numberOfEchangesRegistered = int.Parse(data[1]);
            var tauxDeChanges = new List<TauxDeChange>();
            for (var index = 2; index < 2 + numberOfEchangesRegistered; index++)
            {
                var ligne = data[index].Split(';');
                tauxDeChanges.Add(new TauxDeChange
                {
                    DeviseInitial = ligne[0],
                    DeviseCible = ligne[1],
                    Taux = double.Parse(ligne[2].Replace('.', ',')),
                });
            }
            return new Entree
            {
                DeviseInitiale = deviseInitiale,
                DeviseCible = deviseCible,
                Montant = montant,
                TauxDeChanges = tauxDeChanges,
            };
        }

        public static double GetConvertedAmount(Stack<string> path, List<TauxDeChange> tauxDeChanges, int montant)
        {
            var amountConverted = (double)montant;
            var deviseDebut = path.Pop();
            string deviseCible = path.Pop();
            bool isLastOperation = false;
            while (!isLastOperation)
            {
                if (path.Count < 1)
                {
                    isLastOperation = true;
                }
                var tauxDeChangesFound = tauxDeChanges.Where(x => x.DeviseInitial == deviseDebut && x.DeviseCible == deviseCible).FirstOrDefault();
                if (tauxDeChangesFound != null)
                {
                    amountConverted = tauxDeChangesFound.Taux * amountConverted;
                }
                else
                {
                    tauxDeChangesFound = tauxDeChanges.Where(x => x.DeviseInitial == deviseCible && x.DeviseCible == deviseDebut).FirstOrDefault();
                    amountConverted = (1 / tauxDeChangesFound.Taux) * amountConverted;
                }

                deviseDebut = deviseCible;
                amountConverted = !isLastOperation ? Math.Round(amountConverted, 4) : Math.Round(amountConverted, 0);

                if (!isLastOperation)
                {
                    deviseCible = path.Pop();
                }
            }
            return amountConverted;
        }

        public static Stack<string> GetShortestPath(Graph adjacencyList, string deviseDepart, string deviseCible)
        {
            var (distances, previous, Q) = InitPath(adjacencyList, deviseDepart);
            while (Q.Count > 0)
            {
                var minValue = distances.Values.Min();
                var key = distances.Where(x => x.Value == distances.Values.Min()).First().Key;
                var u = Q.Where(x => x == key).First();
                if (u == deviseCible)
                {
                    break;
                }
                var neighborsStillInQ = adjacencyList
                    .AdjacencyList[u]
                    .Where(x => Q.Contains(x));
                foreach (var neighbor in neighborsStillInQ)
                {
                    var alt = distances[u] + 1;
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = u;
                    }
                }
                Q.RemoveAt(Q.IndexOf(u));
                distances[key] = int.MaxValue;
            }
            return TracePath(previous, deviseDepart, deviseCible);
        }

        private static (Dictionary<string, int> distances, Dictionary<string, string> previousNodes, List<string> Q) InitPath(Graph adjacencyList, string deviseDepart)
        {
            var distances = new Dictionary<string, int>();
            var previous = new Dictionary<string, string>();
            var Q = new List<string>();
            foreach (var node in adjacencyList.AdjacencyList.Keys)
            {
                distances[node] = int.MaxValue;
                previous[node] = null;
                Q.Add(node);
            }
            distances[deviseDepart] = 0;
            return (distances, previous, Q);
        }

        private static Stack<string> TracePath(Dictionary<string, string> nodes, string deviseDepart, string deviseCible)
        {
            Stack<string> sequence = new Stack<string>();
            var temp = deviseCible;
            if (nodes[temp] != null || temp == deviseDepart)
            {
                while (temp != null)
                {
                    sequence.Push(temp);
                    temp = nodes[temp];
                }
            }
            return sequence;
        }

        public static Graph BuildGraph(List<TauxDeChange> tauxDeChanges)
        {
            var graph = new Graph();
            Dictionary<string, List<string>> adjacencyList = new Dictionary<string, List<string>>();
            foreach (var exchange in tauxDeChanges)
            {
                if (!adjacencyList.Keys.Contains(exchange.DeviseInitial))
                {
                    adjacencyList.Add(exchange.DeviseInitial, new List<string> { exchange.DeviseCible });
                }
                if (!adjacencyList.Keys.Contains(exchange.DeviseCible))
                {
                    adjacencyList.Add(exchange.DeviseCible, new List<string> { exchange.DeviseInitial });
                }
                if (adjacencyList.Keys.Contains(exchange.DeviseInitial) && !adjacencyList[exchange.DeviseInitial].Contains(exchange.DeviseCible))
                {
                    adjacencyList[exchange.DeviseInitial].Add(exchange.DeviseCible);
                }
                if (adjacencyList.Keys.Contains(exchange.DeviseCible) && !adjacencyList[exchange.DeviseCible].Contains(exchange.DeviseInitial))
                {
                    adjacencyList[exchange.DeviseCible].Add(exchange.DeviseInitial);
                }
            }
            graph.AdjacencyList = adjacencyList;
            return graph;
        }

        #region Check file format and its content
        public static void CheckArgs(string[] args)
        {
            if (args.Length != 1)
            {
                throw new Exception("Must contain the file path");
            }
            CheckFileExists(args[0]);
            var data = File.ReadAllLines(args[0], encoding: Encoding.UTF8).ToList();
            if (data.Count > 2)
            {
                CheckEntryLine(data[0]);
            }
            string[] firstLineSplit = data[0].Split(';');
            var deviseDepart = firstLineSplit[0];
            var deviseCible = firstLineSplit[2];
            int numberOfEchange = int.Parse(data[1]);
            if ((data.Count - 2) < numberOfEchange)
            {
                throw new Exception("Missing exchanges");
            }
            for (var index = 2; index < data.Count; index++)
            {
                var line = data[index];
                CheckEchangeLine(line);
            }

            data.RemoveAt(0);
            data.RemoveAt(0);
            CheckDeviseExistenceInTable(data, deviseDepart, deviseCible);
        }

        private static void CheckDeviseExistenceInTable(IEnumerable<string> deviseTable, string deviseDepart, string deviseCible)
        {
            bool isDeviseStartExistsInConversionTable = false;
            bool isDeviseEndExistsInConversionTable = false;
            foreach (var line in deviseTable)
            {
                if (line.Contains(deviseDepart))
                {
                    isDeviseStartExistsInConversionTable = true;
                }
                if (line.Contains(deviseCible))
                {
                    isDeviseEndExistsInConversionTable = true;
                }
            }
            if (!isDeviseEndExistsInConversionTable || !isDeviseStartExistsInConversionTable)
            {
                throw new Exception($"There is no record allowing the conversion from {deviseDepart} to {deviseCible}");
            }
        }

        private static void CheckFileExists(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("Cannot load the file content. File does not exists or the path is not the right one !");
            }
        }

        private static void CheckEchangeLine(string line)
        {
            var valueSplit = line.Split(';');
            if (valueSplit.Length != 3)
            {
                throw new Exception("Not enough data");
            }
            var deviseDepart = valueSplit[0];
            var deviseCible = valueSplit[1];
            if (deviseDepart.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {deviseDepart}");
            }
            if (deviseCible.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {deviseCible}");
            }
            double currentValue = -1;
            var amountConverted = valueSplit[2].Replace('.', ',');
            var isParsingFailed = !double.TryParse(amountConverted, out currentValue);
            if (isParsingFailed)
            {
                throw new Exception($"Cannot parsed : {line}");
            }
        }

        private static void CheckEntryLine(string line)
        {
            var valueSplit = line.Split(';');
            if (valueSplit.Length != 3)
            {
                throw new Exception("Not enough data");
            }
            var deviseDepart = valueSplit[0];
            var deviseCible = valueSplit[2];
            if (deviseDepart.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {deviseDepart}");
            }
            if (deviseCible.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {deviseCible}");
            }
            int currentValue = -1;
            var isParsingFailed = !int.TryParse(valueSplit[1], out currentValue);
            if (isParsingFailed)
            {
                throw new Exception($"Cannot parsed : {line}");
            }
            if (currentValue < 0)
            {
                throw new Exception($"The currency value should be positive");
            }
        }
        #endregion
    }
}
