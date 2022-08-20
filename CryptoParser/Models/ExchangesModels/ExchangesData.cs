using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CryptoParser.Models.ExchangesModels
{
   enum TradeType
   {
      Buy,
      Sell,
   }

   public class ExchangesData
   {
      private List<Offer> _offers = new();
      private List<string> _banksNames = new() { "Tinkoff", "RosBank", "RaiffeisenBankRussia", "QIWI", "YandexMoneyNew" };
      private List<string> _currenciesNames = new List<string>() { "USDT", "BTC", "BUSD", "BNB", "ETH" };
      private HttpClient _client = new HttpClient();

      public IReadOnlyList<string> BanksNames => _banksNames.AsReadOnly();
      public IReadOnlyList<string> CurrenciesNames => _currenciesNames.AsReadOnly();
      
      public Offer GetOffer(string exchange, string bank, string currency, Models.TradeType tradeType)
      {
         return _offers.FirstOrDefault(offer => offer.Exchange == exchange &&
                                offer.Bank == bank &&
                                offer.Currency == currency &&
                                offer.TradeType == tradeType);
      }

      public async Task UpdateDataAsync()
      {
         _offers.Clear();
         foreach (string bankName in _banksNames)
         {
            Services.ServicesContainer.Get<Services.Logger>().Log.Info($"Parse Binance {bankName} prices");

            foreach (string currencyName in _currenciesNames)
            {
               Offer buyOffer = await parseOfferAsync(bankName, currencyName, Models.TradeType.Buy);
               _offers.Add(buyOffer);
               Offer sellOffer = await parseOfferAsync(bankName, currencyName, Models.TradeType.Sell);
               _offers.Add(sellOffer);
               Services.ServicesContainer.Get<Services.Logger>().Log.Info($"Binance {bankName} {currencyName} prices parsed");
            }

            Services.ServicesContainer.Get<Services.Logger>().Log.Info($"Binance {bankName} prices parsed");
         }

         Services.ServicesContainer.Get<Services.Logger>().Log.Info("Binance prices parsed");
      }

      private async Task<Offer> parseOfferAsync(string bankName, string assetName, Models.TradeType tradeType)
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
               tradeType = TradeTypesExtensions.TypeToString(tradeType)
            });

         var requestUri = "https://p2p.binance.com/bapi/c2c/v2/friendly/c2c/adv/search";
         var content = new StringContent(data, Encoding.UTF8, "application/json");
         try
         {
            var response = await _client.PostAsync(requestUri, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            var minPrice = responseJson["data"][0]["adv"]["price"];
            return new Offer("Binance", bankName, assetName, tradeType, ((float)minPrice), "OK");
         }
         catch (HttpRequestException e)
         {
            Services.ServicesContainer.Get<Services.Logger>().Log.Info($"Binance parse exeption: {e.Message}");
            return new Offer("Binance", bankName, assetName, tradeType, 0, "BadRequest");
         }
      }
   }
}
