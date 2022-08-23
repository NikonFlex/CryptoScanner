using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoParser.Models
{
   namespace Parsers
   {
      public static class OKXParser
      {
         private static readonly HttpClient _client = new HttpClient();

         public static async Task UpdateDataAsync()
         {
            Logger.Info("Start parse OKX prices");

            await Task.WhenAll(parseOffersAsync(), parseMarketPricesAsync());

            Logger.Info("OKX prices parsed");
         }

         private static async Task parseOffersAsync()
         {
            Logger.Info($"Parse OKX offers process started");

            List<Task<Offer>> tasks = new();

            foreach (string bankName in Constants.BanksNames(ExchangeType.OKX))
            {
               foreach (string currencyName in Constants.CurrenciesNames(ExchangeType.OKX))
               {
                  tasks.Add(parseOfferAsync(bankName, currencyName, TradeType.Buy));
                  tasks.Add(parseOfferAsync(bankName, currencyName, TradeType.Sell));
               }
            }
            Logger.Info($"Parse OKX offers process passed");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(task => ServicesContainer.Get<ExchangesData>().AddOffer(task.Result));

            Logger.Info($"Parse OKX offers process finished");
         }

         private static async Task<Offer> parseOfferAsync(string bankName, string currencyName, TradeType tradeType)
         {
            try
            {
               var requestUri = $"https://www.okx.com/v3/c2c/tradingOrders/books?t={DateTime.UtcNow.Ticks}&quoteCurrency=rub&baseCurrency={currencyName}&side={tradeType.TypeToString()}&paymentMethod={bankName}&userType=all&showTrade=false&showFollow=false&showAlreadyTraded=false&isAbleFilter=false";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);
               float minPrice;
               if (tradeType == TradeType.Buy)
               {
                  minPrice = (float)responseJson["data"]["buy"][0]["price"];
                  return new Offer(ExchangeType.OKX, bankName, currencyName, TradeType.Sell, minPrice, "OK");
               }
               else
               {
                  minPrice = (float)responseJson["data"]["sell"][0]["price"];
                  return new Offer(ExchangeType.OKX, bankName, currencyName, TradeType.Buy, minPrice, "OK");
               }

               Logger.Info($"OKX Offer: {bankName}, {currencyName}, {tradeType.TypeToString()} parsed successfully");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"OKX  parse exeption: {e.Message}");
               return new Offer(ExchangeType.OKX, bankName, currencyName, tradeType, 0, "BadRequest");
            }
         }

         private static async Task parseMarketPricesAsync()
         {
            Logger.Info($"Parse OKX marketPrices process started");

            List<Task<MarketRate>> tasks = new();

            foreach (var currencyName in Constants.CurrenciesNames(ExchangeType.OKX))
               tasks.Add(parseMarketPriceAsync(currencyName));

            Logger.Info($"Parse OKX marketPrices process passed");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(rate => ServicesContainer.Get<ExchangesData>().AddMarketPrice(rate.Result));

            Logger.Info($"Parse OKX marketPrices process finished");
         }

         private static async Task<MarketRate> parseMarketPriceAsync(string currencyName)
         {
            if (currencyName == "USDT")
               return new MarketRate(ExchangeType.OKX, currencyName, 1, "OK");

            try
            {
               var requestUri = $"https://www.okx.com/api/v5/market/ticker?instId={currencyName}-USD-SWAP";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);
               var price = (float)responseJson["data"][0]["last"];

               Logger.Info($"OKX MarketPrice: {currencyName} parsed successfully");

               return new MarketRate(ExchangeType.OKX, currencyName, price, "OK");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"OKX parse exeption: {e.Message}");
               return new MarketRate(ExchangeType.OKX, currencyName, 0, "OK");
            }
         }
      }
   }
}