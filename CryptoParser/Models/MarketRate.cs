namespace CryptoParser.Models
{
   public class MarketRate
   {
      public CVBType CVB { get; private set; }
      public string Currency { get; private set; }
      public float Price { get; private set; }
      public string Message { get; private set; }

      public MarketRate(CVBType cvb, string currency, float price, string message)
      {
         CVB = cvb;
         Currency = currency;
         Price = price;
         Message = message;
      }
   }
}
