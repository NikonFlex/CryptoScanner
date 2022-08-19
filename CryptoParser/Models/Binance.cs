using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CryptoParser.Models
{
   public static class Binance
   {
      private static List<Bank> _banks = new();
      private static List<string> _banksNames = new() { "Tinkoff", "RosBank", "RaiffeisenBankRussia", "QIWI", "YandexMoneyNew" };
      private static List<string> _currenciesNames = new List<string>() { "USDT", "BTC", "BUSD", "BNB", "ETH" };
      private static HttpClient _client = new HttpClient();

      public static IReadOnlyCollection<Bank> Banks => _banks.AsReadOnly();
      public static IReadOnlyCollection<string> BanksNames => _banksNames.AsReadOnly();
      public static IReadOnlyCollection<string> CurrenciesNames => _currenciesNames.AsReadOnly();

      public static async Task ParseCoursesAsync()
      {
         _banks.Clear();
         foreach (string bankName in _banksNames)
         {
            Bank bank = new(bankName);
            Services.ServicesContainer.Get<Services.Logger>().Log.Info($"Parse Binance {bankName} prices");

            foreach (string currencyName in _currenciesNames)
            {
               float buyPrice = await parsePriceAsync(bankName, currencyName, "BUY");
               float sellPrice = await parsePriceAsync(bankName, currencyName, "SELL");
               Services.ServicesContainer.Get<Services.Logger>().Log.Info($"Binance {bankName} {currencyName} prices parsed");

               СryptoСurrency сurrency = new(currencyName, buyPrice, sellPrice);
               bank.AddCurrency(сurrency);
            }

            _banks.Add(bank);
         }

         Services.ServicesContainer.Get<Services.Logger>().Log.Info("Binance prices parsed");
      }

      private static async Task<float> parsePriceAsync(string bankName, string assetName, string tradeType)
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
               tradeType = tradeType
            });

         var requestUri = "https://p2p.binance.com/bapi/c2c/v2/friendly/c2c/adv/search";
         var content = new StringContent(data, Encoding.UTF8, "application/json");
         try
         {
            var response = await _client.PostAsync(requestUri, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            var minPrice = responseJson["data"][0]["adv"]["price"];
            return ((float)minPrice);
         }
         catch (HttpRequestException e)
         {
            Services.ServicesContainer.Get<Services.Logger>().Log.Info($"Binance parse exeption: {e.Message}");
            return 0;
         }
      }
   }
}
