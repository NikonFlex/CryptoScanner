namespace CryptoParser.Models
{
   public static class Utils
   {
      public static ExchangeType GetExchangeTypeFrom(string exchange)
      {
         if (exchange == "Binance")
            return ExchangeType.Binance;
         if (exchange == "OKX")
            return ExchangeType.OKX;
         else
            throw new Exception($"Cannot convert {exchange} to exchange");
      }
   }
}
