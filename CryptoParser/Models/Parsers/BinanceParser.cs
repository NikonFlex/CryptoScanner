using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CryptoParser.Models
{
   namespace Parsers
   {
      public static class BinanceParser
      {
         private static readonly HttpClient _client = new HttpClient();

         public static async Task UpdateDataAsync()
         {
            foreach (string bankName in Constants.BanksNames)
            {
               Services.Logger.Instance.Log.Info($"Parse Binance {bankName} prices");

               foreach (string currencyName in Constants.CurrenciesNames)
               {
                  Offer buyOffer = await parseOfferAsync(bankName, currencyName, TradeType.Buy);
                  Services.ServicesContainer.Get<ExchangesData>().AddOffer(buyOffer);
                  Offer sellOffer = await parseOfferAsync(bankName, currencyName, TradeType.Sell);
                  Services.ServicesContainer.Get<ExchangesData>().AddOffer(sellOffer);

                  Services.Logger.Instance.Log.Info($"Binance {bankName} {currencyName} prices parsed");
               }

               Services.Logger.Instance.Log.Info($"Binance {bankName} prices parsed");
            }

            Services.Logger.Instance.Log.Info("Binance prices parsed");
         }

         private static async Task<Offer> parseOfferAsync(string bankName, string assetName, TradeType tradeType)
         {
            string data = JsonConvert.SerializeObject(
               new
               {
                  asset = assetName,
                  fiat = "RUB",
                  merchantCheck = false,
                  page = 1,
                  payTypes = new[] { bankName },
                  rows = 1,
                  tradeType = tradeType.TypeToString()
               });

            var requestUri = "https://p2p.binance.com/bapi/c2c/v2/friendly/c2c/adv/search";
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            try
            {
               var response = await _client.PostAsync(requestUri, content);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);
               var minPrice = responseJson["data"][0]["adv"]["price"];
               return new Offer("Binance", bankName, assetName, tradeType, (float)minPrice, "OK");
            }
            catch (HttpRequestException e)
            {
               Services.Logger.Instance.Log.Info($"Binance parse exeption: {e.Message}");
               return new Offer("Binance", bankName, assetName, tradeType, 0, "BadRequest");
            }
         }
      }
   }
}
