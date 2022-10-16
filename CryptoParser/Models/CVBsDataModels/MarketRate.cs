using Newtonsoft.Json;

namespace CryptoParser
{
   namespace Models
   {
      public class MarketRate
      {
         [JsonProperty]
         public CVBType CVB { get; private set; }
         [JsonProperty]
         public Currency Currency { get; private set; }
         [JsonProperty]
         public float Price { get; private set; }
         [JsonProperty]
         public string Message { get; private set; }

         public MarketRate(CVBType cvb, Currency currency, float price, string message)
         {
            CVB = cvb;
            Currency = currency;
            Price = price;
            Message = message;
         }
      }
   }
}
