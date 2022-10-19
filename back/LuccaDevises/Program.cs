namespace LuccaDevises
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        class RegistreConvertisseur
        {
            public Transaction TransactionSouhaite { get; set; }
            public List<Conversion> TableDesConversions { get; set; }
            
            public RegistreConvertisseur(string[] args)
            {
                var provider = new LuccaDevises();
                if (args.Length != 1)
                {
                    Console.WriteLine($"Wrong number of arguments");
                }
                try
                {
                    var result = provider.LoadEntryFile(args[0]);
                    TransactionSouhaite = result.Item1;
                    TableDesConversions = result.Item2;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void Main(string[] args)
        {
            var registreConvertisseur = new RegistreConvertisseur(args);
            var luccaDevises = new LuccaDevises();
            var adjacencyList = luccaDevises.BuildGraph(registreConvertisseur.TableDesConversions);
            var path = luccaDevises.GetShortestPath(adjacencyList, registreConvertisseur.TransactionSouhaite.DeviseDepart, registreConvertisseur.TransactionSouhaite.DeviseCible);
            Console.WriteLine(path);
        }
    }
}
