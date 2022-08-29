namespace CryptoParser.Models
{
   public class Offer
   {
      public CVBType CVB { get; private set; }
      public string Bank { get; private set; }
      public string Currency { get; private set; }
      public TradeType TradeType { get; private set; }
      public float Price { get; private set; }
      public string Message { get; private set; }

      public Offer(CVBType cvb, string bank, string currency, TradeType type, float price, string message)
      {
         CVB = cvb;
         Bank = bank;
         Currency = currency;
         TradeType = type;
         Price = price;
         Message = message;
      }
   }
}
