namespace LuccaDevises
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            LuccaDevisesHelper.CheckArgs(args);
            Entree entree = LuccaDevisesHelper.LireEntree(args);
            Graph adjacencyList = LuccaDevisesHelper.BuildGraph(entree.TauxDeChanges);
            Stack<string> path = LuccaDevisesHelper.GetShortestPath(adjacencyList, entree.DeviseInitiale, entree.DeviseCible);
            double amountConverted = LuccaDevisesHelper.GetConvertedAmount(path, entree.TauxDeChanges, entree.Montant);
            Console.WriteLine(amountConverted);
        }
    }
}
