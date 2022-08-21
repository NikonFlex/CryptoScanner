using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using CryptoParser.Services;

namespace CryptoParser.Models
{
   namespace Parsers
   {
      public static class BinanceParser
      {
         private static readonly HttpClient _client = new HttpClient();

         public static async Task UpdateDataAsync()
         {
            Logger.Info("Start parse Binance prices");

            await Task.WhenAll(parseOffersAsync(), parseMarketPricesAsync());

            Logger.Info("Binance prices parsed");
         }

         private static async Task parseOffersAsync()
         {
            Logger.Info($"Parse offers process started");

            List<Task<Offer>> tasks = new();

            foreach (string bankName in Constants.BanksNames)
            {
               foreach (string currencyName in Constants.CurrenciesNames)
               {
                  tasks.Add(parseOfferAsync(bankName, currencyName, TradeType.Buy));
                  tasks.Add(parseOfferAsync(bankName, currencyName, TradeType.Sell));
               }
            }
            Logger.Info($"Parse offers process passed");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(task => ServicesContainer.Get<ExchangesData>().AddOffer(task.Result));

            Logger.Info($"Parse offers process finished");
         }

         private static async Task<Offer> parseOfferAsync(string bankName, string currencyName, TradeType tradeType)
         {
            string data = JsonConvert.SerializeObject(
               new
               {
                  asset = currencyName,
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

               Logger.Info($"Offer: {bankName}, {currencyName}, {tradeType.TypeToString()} parsed successfully");

               return new Offer("Binance", bankName, currencyName, tradeType, (float)minPrice, "OK");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"Binance parse exeption: {e.Message}");
               return new Offer("Binance", bankName, currencyName, tradeType, 0, "BadRequest");
            }
         }

         private static async Task parseMarketPricesAsync()
         {
            Logger.Info($"Parse marketPrices process started");

            List<Task<MarketRate>> tasks = new();

            foreach (var currencyName in Constants.CurrenciesNames)
               tasks.Add(parseMarketPriceAsync(currencyName));
            
            Logger.Info($"Parse marketPrices process passed");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(rate => ServicesContainer.Get<ExchangesData>().AddMarketPrice(rate.Result));

            Logger.Info($"Parse marketPrices process finished");
         }

         private static async Task<MarketRate> parseMarketPriceAsync(string currencyName)
         {
            if (currencyName == "USDT")
               return new MarketRate("Binance", currencyName, 1, "OK");

            var requestUri = $"https://api.binance.com/api/v3/ticker/price?symbol={currencyName}USDT";
            try
            {
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);
               var price = (float)responseJson["price"];

               Logger.Info($"MarketPrice: {currencyName} parsed successfully");

               return new MarketRate("Binance", currencyName, price, "OK");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"Binance parse exeption: {e.Message}");
               return new MarketRate("Binance", currencyName, 0, "OK");
            }
         }
      }
   }
}
