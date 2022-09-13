namespace CryptoParser.Models
{
   public class Offer
   {
      public CVBType CVB { get; private set; }
      public Bank Bank { get; private set; }
      public Currency Currency { get; private set; }
      public TradeType TradeType { get; private set; }
      public float Price { get; private set; }
      public string Message { get; private set; }

      public Offer(CVBType cvb, Bank bank, Currency currency, TradeType type, float price, string message)
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
