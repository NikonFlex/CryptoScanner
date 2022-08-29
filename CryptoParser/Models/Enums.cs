namespace CryptoParser.Models
{
   public enum CVBType
   {
      Binance,
      OKX,
      Bybit,
   }

   public enum TradeType
   { 
      Buy,
      Sell,
   }

   public enum SpreadType
   {
      Rub,
      Percent
   }

   static class EnumExtensions
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
