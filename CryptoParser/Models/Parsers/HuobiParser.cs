using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoParser.Models
{
   namespace Parsers
   {
      [Parser]
      public class HuobiParser : IParser
      {
         private readonly HttpClient _client = new HttpClient();
         private CVBData _cvbData;

         public HuobiParser()
         {
            _cvbData = Constants.GetCVBData(CVBType.Huobi);
         }

         public async Task UpdateDataAsync()
         {
            Logger.Info("Start parse Huobi prices");

            await Task.WhenAll(parseOffersAsync(), parseMarketPricesAsync());

            Logger.Info("Huobi prices parsed");
         }

         private async Task parseOffersAsync()
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
            tasks.ForEach(task => ServicesContainer.Get<CVBsData>().AddOffer(task.Result));

            Logger.Info($"Parse Huobi offers process finished");
         }

         private async Task<Offer> parseOfferAsync(Bank bank, Currency currency, TradeType tradeType)
         {
            try
            {
               var requestUri = $"https://otc-api.trygofast.com/v1/data/trade-market?coinId={Utils.GetCurrencyNameFrom(currency, _cvbData.CVB)}&currency=11&tradeType={tradeType.TypeToString()}&payMethod={Utils.GetBankNameFrom(bank, _cvbData.CVB)}&blockType=general";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);
               float minPrice;
               minPrice = (float)responseJson["data"][0]["price"];
               
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

         private async Task parseMarketPricesAsync()
         {
            Logger.Info($"Parse Huobi marketPrices process started");

            List<Task<MarketRate>> tasks = new();
            _cvbData.Currencies.ToList().ForEach(currency => tasks.Add(parseMarketPriceAsync(currency)));

            Logger.Info($"Parse Huobi marketPrices process passed");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(rate => ServicesContainer.Get<CVBsData>().AddMarketPrice(rate.Result));

            Logger.Info($"Parse Huobi marketPrices process finished");
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
               var responseJson = JObject.Parse(responseString);
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