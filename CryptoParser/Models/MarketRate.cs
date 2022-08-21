namespace CryptoParser.Models
{
   public class MarketRate
   {
      public string Exchange { get; private set; }
      public string Currency { get; private set; }
      public float Price { get; private set; }
      public string Message { get; private set; }

      public MarketRate(string exchange, string currency, float price, string message)
      {
         Exchange = exchange;
         Currency = currency;
         Price = price;
         Message = message;
      }
   }
}
