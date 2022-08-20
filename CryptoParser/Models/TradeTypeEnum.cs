namespace CryptoParser.Models
{
   public enum TradeType
   { 
      Buy,
      Sell,
   }

   static class TradeTypesExtensions
   {
      public static string TypeToString(this TradeType type)
      {
         switch (type)
         {
            case TradeType.Buy: return "BUY";
            case TradeType.Sell: return "SELL";
            default: throw new ArgumentOutOfRangeException("type");
         }
      }
   }
}
