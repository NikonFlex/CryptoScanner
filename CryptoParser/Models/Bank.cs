namespace CryptoParser.Models
{
   public class Bank
   {
      public string Name { get; private set; }

      private List<СryptoСurrency> _currencies = new();

      public IReadOnlyCollection<СryptoСurrency> Currencies => _currencies.AsReadOnly();

      public Bank(string name)
      {
         Name = name;
      }

      public void AddCurrency(СryptoСurrency сurrency) => _currencies.Add(сurrency);
   }
}
