using CryptoParser.Models;

namespace CryptoParser
{
   namespace Parsing
   {
      [Parser]
      public class OKXParser : IParser
      {
         private readonly HttpClient _client = new HttpClient();
         private CVBData _cvbData;

         public OKXParser()
         {
            _cvbData = Constants.GetCVBData(CVBType.OKX);
         }

         public async Task<List<Offer>> ParseOffersAsync()
         {
            Logger.Info($"Parse OKX offers process started");

            List<Task<Offer>> tasks = new();

            foreach (Bank bank in _cvbData.Banks)
            {
               foreach (Currency currency in _cvbData.Currencies)
               {
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Buy));
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Sell));
               }
            }
            Logger.Info($"Parse OKX offers process passed");

            await Task.WhenAll(tasks.ToArray());
            List<Offer> offers = new();
            tasks.ForEach(task => offers.Add(task.Result));

            Logger.Info($"Parse OKX offers process finished");

            return offers;
         }

         private async Task<Offer> parseOfferAsync(Bank bank, Currency currency, TradeType tradeType)
         {
            try
            {
               var requestUri = $"https://www.okx.com/v3/c2c/tradingOrders/books?t={DateTime.UtcNow.Ticks}&quoteCurrency=rub&baseCurrency={Utils.GetCurrencyNameFrom(currency, _cvbData.CVB)}&side={tradeType.TypeToString()}&paymentMethod={Utils.GetBankNameFrom(bank, _cvbData.CVB)}&userType=all&showTrade=false&showFollow=false&showAlreadyTraded=false&isAbleFilter=false";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = Newtonsoft.Json.Linq.JObject.Parse(responseString);

               var adverts = responseJson["data"][tradeType == TradeType.Buy ? "buy" : "sell"];
               var advertsCountToMedian = adverts.Count() > 5 ? 5 : adverts.Count();
               List<float> prices = new();
               for (int i = 0; i < advertsCountToMedian; i++)
                  prices.Add((float)adverts[i]["price"]);
               prices.Sort();
               float minPrice = prices[(int)prices.Count() / 2];
               
               return new Offer(_cvbData.CVB, bank, currency, tradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, minPrice, "OK");

               Logger.Info($"OKX Offer: {bank}, {currency}, {tradeType.TypeToString()} parsed successfully");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"OKX parse exeption: {e.Message}");
               return new Offer(_cvbData.CVB, bank, currency, tradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"OKX read answer exeption: {e.Message}");
               return new Offer(_cvbData.CVB, bank, currency, tradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, 0, e.Message);
            }
         }

         public async Task<List<MarketRate>> ParseMarketRatesAsync()
         {
            Logger.Info($"Parse OKX marketPrices process started");

            List<Task<MarketRate>> tasks = new();
            _cvbData.Currencies.ToList().ForEach(currency => tasks.Add(parseMarketPriceAsync(currency)));
            
            Logger.Info($"Parse OKX marketPrices process passed");

            await Task.WhenAll(tasks.ToArray());
            List<MarketRate> marketRates = new();
            tasks.ForEach(rate => marketRates.Add(rate.Result));

            Logger.Info($"Parse OKX marketPrices process finished");

            return marketRates;
         }

         private async Task<MarketRate> parseMarketPriceAsync(Currency currency)
         {
            if (currency == Currency.USDT)
               return new MarketRate(CVBType.OKX, currency, 1, "OK");

            try
            {
               var requestUri = $"https://www.okx.com/api/v5/market/ticker?instId={Utils.GetCurrencyNameFrom(currency, _cvbData.CVB)}-USD-SWAP";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = Newtonsoft.Json.Linq.JObject.Parse(responseString);
               var price = (float)responseJson["data"][0]["last"];

               Logger.Info($"OKX MarketPrice: {currency} parsed successfully");

               return new MarketRate(_cvbData.CVB, currency, price, "OK");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"OKX parse exeption: {e.Message}");
               return new MarketRate(_cvbData.CVB, currency, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"OKX read answer exeption: {e.Message}");
               return new MarketRate(_cvbData.CVB, currency, 0, e.Message);
            }
         }
      }
   }
}