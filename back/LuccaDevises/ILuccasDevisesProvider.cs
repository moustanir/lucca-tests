namespace LuccaDevises
{
    using System.Collections.Generic;

    public interface ILuccasDevisesProvider
    {
        (Transaction, List<Conversion>) LoadEntryFile(string filename);  
    }
}
