using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CryptoParser.Models
{
   public static class Parser
   {
      private static HttpClient _client = new HttpClient();
            
      public static async Task<float> ParsePrice()
      {
         string data = JsonConvert.SerializeObject(
            new 
            {
               asset = "USDT",
               fiat = "RUB",
               merchantCheck = false,
               page = 1,
               payTypes = new[] { "Tinkoff" },
               rows = 1,
               tradeType = "BUY"
            });

         var requestUri = "https://p2p.binance.com/bapi/c2c/v2/friendly/c2c/adv/search";
         var content = new StringContent(data, Encoding.UTF8, "application/json");
         var response = await _client.PostAsync(requestUri, content);

         var responseString = await response.Content.ReadAsStringAsync();
         var responseJson = JObject.Parse(responseString);
         var minPrice = responseJson["data"][0]["adv"]["price"];
        
         Services.ServicesContainer.Get<Services.Logger>().Log.Info("Binance prices parsed");

         return ((float)minPrice);
      }

      //public static int ReturnPrice(int price) => price;
   }
}
