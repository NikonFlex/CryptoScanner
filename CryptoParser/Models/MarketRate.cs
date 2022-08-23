namespace CryptoParser.Models
{
   public class MarketRate
   {
      public ExchangeType Exchange { get; private set; }
      public string Currency { get; private set; }
      public float Price { get; private set; }
      public string Message { get; private set; }

      public MarketRate(ExchangeType exchange, string currency, float price, string message)
      {
         Exchange = exchange;
         Currency = currency;
         Price = price;
         Message = message;
      }
   }
}
