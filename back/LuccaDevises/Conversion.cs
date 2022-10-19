namespace LuccaDevises
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class Conversion
    {
        public string DeviseDepart { get; set; }
        public string DeviseArrivee { get; set; }
        public double TauxEchange { get; set; }

        public Conversion(string data)
        {
            var valueSplit = data.Split(';');
            if (valueSplit.Length != 3)
            {
                throw new Exception("Not enough data");
            }
            this.DeviseDepart = valueSplit[0];
            this.DeviseArrivee = valueSplit[1];
            if (this.DeviseDepart.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {this.DeviseDepart}");
            }
            if (this.DeviseArrivee.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {this.DeviseArrivee}");
            }
            double currentValue = -1;
            var amountConverted = valueSplit[2].Replace('.', ',');
            // Make a method for this
            var isParsingFailed = !double.TryParse(amountConverted, out currentValue);
            if (isParsingFailed)
            {
                throw new Exception($"Cannot parsed : {data}");
            }
            this.TauxEchange = currentValue;
        }
    }
}
