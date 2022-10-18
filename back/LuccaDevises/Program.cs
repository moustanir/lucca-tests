namespace LuccaDevises
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Devise
    {
        public string Start { get; set; }
        public string Target { get; set; }
        public double ExchangeRate { get; set; }

        public Devise(string data)
        {
            var valueSplit = data.Split(';');
            if (valueSplit.Length != 3)
            {
                throw new Exception("Not enough data");
            }
            this.Start = valueSplit[0];
            this.Target = valueSplit[1];
            if (this.Start.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {this.Start}");
            }
            if (this.Target.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {this.Target}");
            }
            double currentValue = -1;
            var toto = valueSplit[2].Replace('.', ',');
            // Make a method for this
            var isParsingFailed = !double.TryParse(toto, out currentValue);
            if (isParsingFailed)
            {
                throw new Exception($"Cannot parsed : {data}");
            }
            this.ExchangeRate = currentValue;
        }
    }

    public class EntryDevise
    {
        public string StartingDevise { get; set; }
        public string DeviseTargeted { get; set; }
        public int Amount { get; set; }
        
        public EntryDevise(string data)
        {
            var valueSplit = data.Split(';');
            if(valueSplit.Length != 3)
            {
                throw new Exception("Not enough data");
            }
            this.StartingDevise = valueSplit[0];
            this.DeviseTargeted = valueSplit[2];
            if(this.StartingDevise.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {this.StartingDevise}");
            }
            if (this.DeviseTargeted.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {this.DeviseTargeted}");
            }
            // Make a method for this
            int currentValue = -1;
            var isParsingFailed = !int.TryParse(valueSplit[1], out currentValue);
            if (isParsingFailed)
            {
                throw new Exception($"Cannot parsed : {data}");
            }
            if(currentValue < 0)
            {
                throw new Exception($"The currency value should be positive");
            }
            this.Amount = currentValue;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<Devise> devises = new List<Devise>();
                List<string[]> paths = new List<string[]>();
                #region Check entry data coming from the file
                if ((args.Length < 1) || (!File.Exists(args[0])))
                {
                    Console.WriteLine("Cannot load the file content. File does not exists or the path is not the right one !");
                    return;
                }
                var fileName = args[0];
                var data = File.ReadAllLines(fileName, encoding: Encoding.UTF8).ToList();
                if(data.Count < 2)
                {
                    Console.WriteLine("Cannot convert this value. Not enough data");
                    return;
                }
                var entryDevise = new EntryDevise(data[0]);
                data.RemoveAt(0);
                int numberOfDevises = 0;
                var isParsingSucceed = int.TryParse(data[0], out numberOfDevises);
                if(!isParsingSucceed || numberOfDevises < 1)
                {
                    Console.WriteLine("Cannot convert this value. Not enough conversion meter");
                    return;
                }
                data.RemoveAt(0);
                if(data.Count < numberOfDevises)
                {
                    Console.WriteLine("Missing currency conversion");
                    return;
                }
                bool isDeviseStartExistsInConversionTable = false;
                bool isDeviseEndExistsInConversionTable = false;
                foreach (var currencyConversion in data)
                {
                    var devise = new Devise(currencyConversion);
                    if(devise.Start == entryDevise.StartingDevise || devise.Target == entryDevise.StartingDevise)
                    {
                        isDeviseStartExistsInConversionTable = true;
                    }
                    if (devise.Start == entryDevise.DeviseTargeted || devise.Target == entryDevise.DeviseTargeted)
                    {
                        isDeviseEndExistsInConversionTable = true;
                    }
                    devises.Add(devise);
                }
                if(!isDeviseEndExistsInConversionTable || !isDeviseStartExistsInConversionTable)
                {
                    Console.WriteLine($"There is no record allowing the conversion from {entryDevise.StartingDevise} to {entryDevise.DeviseTargeted}");
                    return;
                }
                #endregion
                Dictionary<string, List<string>> adjencyList = new Dictionary<string, List<string>>();
                foreach(var devise in devises)
                {
                    if (!adjencyList.Keys.Contains(devise.Start))
                    {
                        adjencyList.Add(devise.Start, new List<string> { devise.Target });
                    }
                    if(adjencyList.Keys.Contains(devise.Start) && !adjencyList[devise.Start].Contains(devise.Target))
                    {
                        adjencyList[devise.Start].Add(devise.Target);
                    }
                }
                // Check that the currency exists on the list
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
