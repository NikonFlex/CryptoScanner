using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CryptoParser.Models
{
   namespace Parsers
   {
      [Parser]
      public class BinanceParser : IParser
      {
         private readonly HttpClient _client = new HttpClient();
         private CVBData _cvbData;

         public BinanceParser()
         {
            _cvbData = Constants.GetCVBData(CVBType.Binance);
         }

         public async Task UpdateDataAsync()
         {
            Logger.Info("Start parse Binance prices");

            await Task.WhenAll(parseOffersAsync(), parseMarketPricesAsync());

            Logger.Info("Binance prices parsed");
         }

         private async Task parseOffersAsync()
         {
            Logger.Info($"Parse Binance offers process started");

            List<Task<Offer>> tasks = new();

            foreach (string bank in _cvbData.Banks)
            {
               foreach (string currency in _cvbData.Currencies)
               {
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Buy));
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Sell));
               }
            }
            Logger.Info($"Parse Binance offers process passed, {tasks.Count} tasks created");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(task => ServicesContainer.Get<CVBsData>().AddOffer(task.Result));

            Logger.Info($"Parse Binance offers process finished");
         }

         private async Task<Offer> parseOfferAsync(string bank, string currency, TradeType tradeType)
         {

            try
            {
               string data = JsonConvert.SerializeObject(
                  new
                  {
                     asset = currency,
                     fiat = "RUB",
                     merchantCheck = false,
                     page = 1,
                     payTypes = new[] { bank },
                     rows = 1,
                     tradeType = tradeType.TypeToString()
                  });
               var requestUri = "https://p2p.binance.com/bapi/c2c/v2/friendly/c2c/adv/search";
               var content = new StringContent(data, Encoding.UTF8, "application/json");
               var response = await _client.PostAsync(requestUri, content);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);
               var minPrice = responseJson["data"][0]["adv"]["price"];

               Logger.Info($"Binance Offer: {bank}, {currency}, {tradeType.TypeToString()} parsed successfully");

               return new Offer(CVBType.Binance, bank, currency, tradeType, (float)minPrice, "OK");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"Binance parse exeption: {e.Message}");
               return new Offer(CVBType.Binance, bank, currency, tradeType, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"Binance read answer exeption: {e.Message}");
               return new Offer(CVBType.Binance, bank, currency, tradeType, 0, e.Message);
            }
         }

         private async Task parseMarketPricesAsync()
         {
            Logger.Info($"Parse Binance marketPrices process started");

            List<Task<MarketRate>> tasks = new();
            _cvbData.Currencies.ToList().ForEach(currency => tasks.Add(parseMarketPriceAsync(currency)));
            
            Logger.Info($"Parse Binance marketPrices process passed, {tasks.Count} tasks created");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(rate => ServicesContainer.Get<CVBsData>().AddMarketPrice(rate.Result));

            Logger.Info($"Parse Binance marketPrices process finished");
         }

         private async Task<MarketRate> parseMarketPriceAsync(string currency)
         {
            if (currency == "USDT")
               return new MarketRate(CVBType.Binance, currency, 1, "OK");

            try
            {
               var requestUri = $"https://api.binance.com/api/v3/ticker/price?symbol={currency}USDT";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);
               var price = (float)responseJson["price"];

               Logger.Info($"Binance MarketPrice: {currency} parsed successfully");

               return new MarketRate(CVBType.Binance, currency, price, "OK");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"Binance parse exeption: {e.Message}");
               return new MarketRate(CVBType.Binance, currency, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"Binance read answer exeption: {e.Message}");
               return new MarketRate(CVBType.Binance, currency, 0, e.Message);
            }
         }
      }
   }
}
