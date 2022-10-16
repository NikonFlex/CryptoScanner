using Newtonsoft.Json;

namespace CryptoParser
{
   namespace Models
   {
      public class Offer
      {
         [JsonProperty]
         public CVBType CVB { get; private set; }
         [JsonProperty]
         public Bank Bank { get; private set; }
         [JsonProperty]
         public Currency Currency { get; private set; }
         [JsonProperty]
         public TradeType TradeType { get; private set; }
         [JsonProperty]
         public float Price { get; private set; }
         [JsonProperty]
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
}
