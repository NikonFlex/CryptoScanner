namespace CryptoParser.Models
{
   public static class Utils
   {
      public static CVBType GetCVBTypeFrom(string cvb)
      {
         switch (cvb)
         {
            case "Binance":
               return CVBType.Binance;
            case "OKX":
               return CVBType.OKX;
            default:
               throw new Exception($"Cannot convert {cvb} to cvb");
         }
      }

      public static SpreadType GetSpreadTypeFrom(string type)
      {
         switch (type)
         {
            case "РУБ":
               return SpreadType.Rub;
            case "%":
               return SpreadType.Percent;
            default:
               throw new Exception($"Cannot convert {type} to cvb");
         }
      }
   }
}
