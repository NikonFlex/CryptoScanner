using System.Text;
using CryptoParser.Models;

namespace CryptoParser
{
   namespace Parsing
   {
      [Parser]
      public class BinanceParser : IParser
      {
         private readonly HttpClient _client = new();
         private CVBData _cvbData;

         public BinanceParser()
         {
            _cvbData = Constants.GetCVBData(CVBType.Binance);
         }

         public async Task<List<Offer>> ParseOffersAsync()
         {
            Logger.Info($"Parse Binance offers process started");

            List<Task<Offer>> tasks = new();

            foreach (Bank bank in _cvbData.Banks)
            {
               foreach (Currency currency in _cvbData.Currencies)
               {
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Buy));
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Sell));
               }
            }
            Logger.Info($"Parse Binance offers process passed, {tasks.Count} tasks created");

            await Task.WhenAll(tasks.ToArray());

            List<Offer> offers = new();
            tasks.ForEach(task => offers.Add(task.Result));
             
            Logger.Info($"Parse Binance offers process finished");

            return offers;
         }

         private async Task<Offer> parseOfferAsync(Bank bank, Currency currency, TradeType tradeType)
         {
            try
            {
               string data = Newtonsoft.Json.JsonConvert.SerializeObject(
                  new
                  {
                     asset = Utils.GetCurrencyNameFrom(currency, _cvbData.CVB),
                     fiat = "RUB",
                     merchantCheck = false,
                     page = 1,
                     payTypes = new[] { Utils.GetBankNameFrom(bank, _cvbData.CVB) },
                     rows = 5,
                     tradeType = tradeType.TypeToString()
                  });
               var requestUri = "https://p2p.binance.com/bapi/c2c/v2/friendly/c2c/adv/search";
               var content = new StringContent(data, Encoding.UTF8, "application/json");
               var response = await _client.PostAsync(requestUri, content);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = Newtonsoft.Json.Linq.JObject.Parse(responseString);

               var adverts = responseJson["data"];
               List<float> prices = new();
               foreach (var advert in adverts)
                  prices.Add((float)advert["adv"]["price"]);
               prices.Sort();
               var minPrice = prices[((int)(adverts.Count() / 2))];

               Logger.Info($"Binance Offer: {bank}, {currency}, {tradeType.TypeToString()} parsed successfully");

               return new Offer(_cvbData.CVB, bank, currency, tradeType, (float)minPrice, "OK");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"Binance parse exeption: {e.Message}");
               return new Offer(_cvbData.CVB, bank, currency, tradeType, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"Binance read answer exeption: {e.Message}");
               return new Offer(_cvbData.CVB, bank, currency, tradeType, 0, e.Message);
            }
         }

         public async Task<List<MarketRate>> ParseMarketRatesAsync()
         {
            Logger.Info($"Parse Binance marketPrices process started");

            List<Task<MarketRate>> tasks = new();
            _cvbData.Currencies.ToList().ForEach(currency => tasks.Add(parseMarketPriceAsync(currency)));
            
            Logger.Info($"Parse Binance marketPrices process passed, {tasks.Count} tasks created");

            await Task.WhenAll(tasks.ToArray());

            List<MarketRate> marketRates = new();
            tasks.ForEach(rate => marketRates.Add(rate.Result));

            Logger.Info($"Parse Binance marketPrices process finished");

            return marketRates;
         }

         private async Task<MarketRate> parseMarketPriceAsync(Currency currency)
         {
            if (currency == Currency.USDT)
               return new MarketRate(_cvbData.CVB, currency, 1, "OK");

            try
            {
               var requestUri = $"https://api.binance.com/api/v3/ticker/price?symbol={Utils.GetCurrencyNameFrom(currency, _cvbData.CVB)}USDT";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = Newtonsoft.Json.Linq.JObject.Parse(responseString);
               var price = (float)responseJson["price"];

               Logger.Info($"Binance MarketPrice: {currency} parsed successfully");

               return new MarketRate(_cvbData.CVB, currency, price, "OK");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"Binance parse exeption: {e.Message}");
               return new MarketRate(_cvbData.CVB, currency, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"Binance read answer exeption: {e.Message}");
               return new MarketRate(_cvbData.CVB, currency, 0, e.Message);
            }
         }
      }
   }
}
