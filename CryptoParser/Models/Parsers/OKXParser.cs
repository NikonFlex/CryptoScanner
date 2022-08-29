using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoParser.Models
{
   namespace Parsers
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

         public async Task UpdateDataAsync()
         {
            Logger.Info("Start parse OKX prices");

            await Task.WhenAll(parseOffersAsync(), parseMarketPricesAsync());

            Logger.Info("OKX prices parsed");
         }

         private async Task parseOffersAsync()
         {
            Logger.Info($"Parse OKX offers process started");

            List<Task<Offer>> tasks = new();

            foreach (string bank in _cvbData.Banks)
            {
               foreach (string currency in _cvbData.Currencies)
               {
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Buy));
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Sell));
               }
            }
            Logger.Info($"Parse OKX offers process passed");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(task => ServicesContainer.Get<CVBsData>().AddOffer(task.Result));

            Logger.Info($"Parse OKX offers process finished");
         }

         private async Task<Offer> parseOfferAsync(string bank, string currency, TradeType tradeType)
         {
            try
            {
               var requestUri = $"https://www.okx.com/v3/c2c/tradingOrders/books?t={DateTime.UtcNow.Ticks}&quoteCurrency=rub&baseCurrency={currency}&side={tradeType.TypeToString()}&paymentMethod={bank}&userType=all&showTrade=false&showFollow=false&showAlreadyTraded=false&isAbleFilter=false";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);
               float minPrice;

               minPrice = (float)responseJson["data"][tradeType == TradeType.Buy ? "buy" : "sell"][0]["price"];
               return new Offer(CVBType.OKX, bank, currency, tradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, minPrice, "OK");

               Logger.Info($"OKX Offer: {bank}, {currency}, {tradeType.TypeToString()} parsed successfully");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"OKX parse exeption: {e.Message}");
               return new Offer(CVBType.OKX, bank, currency, tradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"OKX read answer exeption: {e.Message}");
               return new Offer(CVBType.OKX, bank, currency, tradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, 0, e.Message);
            }
         }

         private async Task parseMarketPricesAsync()
         {
            Logger.Info($"Parse OKX marketPrices process started");

            List<Task<MarketRate>> tasks = new();
            _cvbData.Currencies.ToList().ForEach(currency => tasks.Add(parseMarketPriceAsync(currency)));
            
            Logger.Info($"Parse OKX marketPrices process passed");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(rate => ServicesContainer.Get<CVBsData>().AddMarketPrice(rate.Result));

            Logger.Info($"Parse OKX marketPrices process finished");
         }

         private async Task<MarketRate> parseMarketPriceAsync(string currency)
         {
            if (currency == "USDT")
               return new MarketRate(CVBType.OKX, currency, 1, "OK");

            try
            {
               var requestUri = $"https://www.okx.com/api/v5/market/ticker?instId={currency}-USD-SWAP";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);
               var price = (float)responseJson["data"][0]["last"];

               Logger.Info($"OKX MarketPrice: {currency} parsed successfully");

               return new MarketRate(CVBType.OKX, currency, price, "OK");
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"OKX parse exeption: {e.Message}");
               return new MarketRate(CVBType.OKX, currency, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"OKX read answer exeption: {e.Message}");
               return new MarketRate(CVBType.OKX, currency, 0, e.Message);
            }
         }
      }
   }
}