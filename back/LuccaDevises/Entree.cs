namespace LuccaDevises
{
    using System.Collections.Generic;
    
    public class Entree
    {
        public string DeviseInitiale { get; set; }
        public string DeviseCible { get; set; }
        public int Montant { get; set; }
        public List<TauxDeChange> TauxDeChanges { get; set; }
    }
}
