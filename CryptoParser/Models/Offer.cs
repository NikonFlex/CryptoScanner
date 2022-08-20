namespace CryptoParser.Models
{
   public class Offer
   {
      public string Exchange { get; private set; }
      public string Bank { get; private set; }
      public string Currency { get; private set; }
      public TradeType TradeType { get; private set; }
      public float Price { get; private set; }
      public string Message { get; private set; }

      public Offer(string exchange, string bank, string currency, TradeType type, float price, string message)
      {
         Exchange = exchange;
         Bank = bank;
         Currency = currency;
         TradeType = type;
         Price = price;
         Message = message;
      }
   }
}
