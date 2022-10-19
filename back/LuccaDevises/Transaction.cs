namespace LuccaDevises
{
    using System;

    public class Transaction
    {
        public string DeviseDepart { get; set; }
        public string DeviseCible { get; set; }
        public int Amount { get; set; }

        public Transaction(string data)
        {
            var valueSplit = data.Split(';');
            if (valueSplit.Length != 3)
            {
                throw new Exception("Not enough data");
            }
            this.DeviseDepart = valueSplit[0];
            this.DeviseCible = valueSplit[2];
            if (this.DeviseDepart.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {this.DeviseDepart}");
            }
            if (this.DeviseCible.Length != 3)
            {
                throw new Exception($"This currency doesn't respect the max length: {this.DeviseCible}");
            }
            // Make a method for this
            int currentValue = -1;
            var isParsingFailed = !int.TryParse(valueSplit[1], out currentValue);
            if (isParsingFailed)
            {
                throw new Exception($"Cannot parsed : {data}");
            }
            if (currentValue < 0)
            {
                throw new Exception($"The currency value should be positive");
            }
            this.Amount = currentValue;
        }
    }
}
