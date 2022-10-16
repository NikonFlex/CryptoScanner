using CryptoParser.Models;

namespace CryptoParser
{
   namespace Parsing
   {
      [Parser]
      public class HuobiParser : IParser
      {
         private readonly HttpClient _client = new();
         private CVBData _cvbData;

         public HuobiParser()
         {
            _cvbData = Constants.GetCVBData(CVBType.Huobi);
         }

         public async Task<List<Offer>> ParseOffersAsync()
         {
            Logger.Info($"Parse Huobi offers process started");

            List<Task<Offer>> tasks = new();

            foreach (Bank bank in _cvbData.Banks)
            {
               foreach (Currency currency in _cvbData.Currencies)
               {
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Buy));
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Sell));
               }
            }
            Logger.Info($"Parse Huobi offers process passed");

            await Task.WhenAll(tasks.ToArray());

            List<Offer> offers = new();
            tasks.ForEach(task => offers.Add(task.Result));

            Logger.Info($"Parse Huobi offers process finished");

            return offers;
         }

         private async Task<Offer> parseOfferAsync(Bank bank, Currency currency, TradeType tradeType)
         {
            try
            {
               var requestUri = $"https://otc-api.trygofast.com/v1/data/trade-market?coinId={Utils.GetCurrencyNameFrom(currency, _cvbData.CVB)}&currency=11&tradeType={tradeType.TypeToString()}&payMethod={Utils.GetBankNameFrom(bank, _cvbData.CVB)}&blockType=general";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = Newtonsoft.Json.Linq.JObject.Parse(responseString);

               var adverts = responseJson["data"];
               var advertsCountToMedian = adverts.Count() > 5 ? 5 : adverts.Count();
               List<float> prices = new();
               for (int i = 0; i < advertsCountToMedian; i++)
                  prices.Add((float)adverts[i]["price"]);
               prices.Sort();
               float minPrice = prices[(int)prices.Count() / 2];
               
               return new Offer(_cvbData.CVB, bank, currency, tradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, minPrice, "OK");

               Logger.Info($"Huobi Offer: {bank}, {currency}, {tradeType.TypeToString()} parsed successfully");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"Huobi parse exeption: {e.Message}");
               return new Offer(_cvbData.CVB, bank, currency, tradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"Huobi read answer exeption: {e.Message}");
               return new Offer(_cvbData.CVB, bank, currency, tradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, 0, e.Message);
            }
         }

         public async Task<List<MarketRate>> ParseMarketRatesAsync()
         {
            Logger.Info($"Parse Huobi marketPrices process started");

            List<Task<MarketRate>> tasks = new();
            _cvbData.Currencies.ToList().ForEach(currency => tasks.Add(parseMarketPriceAsync(currency)));

            Logger.Info($"Parse Huobi marketPrices process passed");

            await Task.WhenAll(tasks.ToArray());

            List<MarketRate> marketRates = new();
            tasks.ForEach(rate => marketRates.Add(rate.Result));

            Logger.Info($"Parse Huobi marketPrices process finished");

            return marketRates;
         }

         private async Task<MarketRate> parseMarketPriceAsync(Currency currency)
         {
            if (currency == Currency.USDT)
               return new MarketRate(CVBType.Huobi, currency, 1, "OK");

            try
            {
               var requestUri = "https://api.huobi.pro/market/tickers";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = Newtonsoft.Json.Linq.JObject.Parse(responseString);
               float price = 0;
               foreach (var symbol in responseJson["data"])
               {
                  if (symbol["symbol"].ToString() == $"{Utils.GetCurrencyNameFrom(currency, CVBType.OKX).ToLower()}usdt")
                     price = (float)symbol["bid"];
               }
               Logger.Info($"Huobi MarketPrice: {currency} parsed successfully");

               return new MarketRate(_cvbData.CVB, currency, price, "OK");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"Huobi parse exeption: {e.Message}");
               return new MarketRate(_cvbData.CVB, currency, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"Huobi read answer exeption: {e.Message}");
               return new MarketRate(_cvbData.CVB, currency, 0, e.Message);
            }
         }
      }
   }
}