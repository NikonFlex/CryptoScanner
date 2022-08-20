using CryptoParser.Models.ExchangesModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using CryptoParser.Services;

namespace CryptoParser.Models
{
   public class GoogleSheetFiller
   {
      private static readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
      private static readonly string _applicationName = "ExchangesInfo";
      private static readonly string _spreadSheetId = "1hr3GQofkjQ62P1k4hwX1N2CpOWJpu8wqiE4Kx-IT8MM";
      private static readonly string _sheet = "Binance";
      private static SheetsService _service;
      private readonly Dictionary<string, string> EngToRusDict = new() 
      {
         { "Tinkoff", "Тинькофф" },
         { "RosBank", "Росбанк" },
         { "RaiffeisenBankRussia", "Райфайзен" },
         { "QIWI", "QIWI" },
         { "YandexMoneyNew", "ЮMoney" },
      };

      public GoogleSheetFiller()
      {
         GoogleCredential credential;
         using (var stream = new FileStream("nikoncryptoprototype-92a9ba4e6df8.json", 
                                             FileMode.Open, 
                                             FileAccess.Read))
         {
            credential = GoogleCredential.FromStream(stream)
               .CreateScoped(_scopes);
         }

         _service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
         {
            HttpClientInitializer = credential,
            ApplicationName = _applicationName,
         });
      }

      public void UpdateSheet()
      {
         updateRatesTable();
         updateCurrencyTable(ServicesContainer.Get<ExchangesData>().CurrenciesNames[0]);
      }

      private void updateCurrencyTable(string currencyName)
      {
         createCurrencyHeaderRow(currencyName);
         int rowCounter = 9;
         foreach (var sellBank in ServicesContainer.Get<ExchangesData>().BanksNames)
         {
            createCurrencyTableRow(currencyName, sellBank, rowCounter);
            rowCounter++;
         }
      }



      private void createCurrencyHeaderRow(string currencyName)
      {
         string tableName = currencyName;

         var range = $"{_sheet}!A8:G8";
         var rowNames = new List<object>() { tableName };
         foreach (var bank in ServicesContainer.Get<ExchangesData>().BanksNames)
            rowNames.Add("Покупка" + "\n" + $"{EngToRusDict[$"{bank}"]}");
         rowNames.Add(DateTime.Now);

         updateRangeValues(rowNames, range);
      }

      private void createCurrencyTableRow(string currencyName, string sellBank, int rowN)
      {
         var range = $"{_sheet}!A{rowN}:F{rowN}";
         var exchangesData = ServicesContainer.Get<ExchangesData>();
         var spreads = new List<object>();
         spreads.Add("Продажа" + "\n" + $"{EngToRusDict[$"{sellBank}"]}");
         foreach (var buyBank in exchangesData.BanksNames)
         {
            var buyOfferPrice = exchangesData.GetOffer("Binance", buyBank, currencyName, TradeType.Buy).Price;
            var sellOfferPrice = exchangesData.GetOffer("Binance", sellBank, currencyName, TradeType.Sell).Price;

            spreads.Add(100000 / sellOfferPrice * buyOfferPrice - 100000);
         }  
         updateRangeValues(spreads, range);
      }

      private void updateRatesTable()
      {
         createRatesHeaderRow();

         int rowCounter = 2;
         foreach (var currencyName in ServicesContainer.Get<ExchangesData>().CurrenciesNames)
         {
            createCurrencyRow(currencyName, rowCounter);
            rowCounter++;
         }
      }

      private void createRatesHeaderRow()
      {
         string tableName = "RUB Курсы:";

         var range = $"{_sheet}!A1:L1";
         var rowNames = new List<object>() { tableName };
         foreach (var bank in ServicesContainer.Get<ExchangesData>().BanksNames)
         {
            rowNames.Add("Покупка" + "\n" + $"{EngToRusDict[$"{bank}"]}");
            rowNames.Add("Продажа" + "\n" + $"{EngToRusDict[$"{bank}"]}");
         }
         rowNames.Add(DateTime.Now);

         updateRangeValues(rowNames, range);
      }

      private void createCurrencyRow(string currencyName, int rowN)
      {
         string rowName = currencyName;

         var range = $"{_sheet}!A{rowN}:K{rowN}";
         var rates = new List<object>() { rowName };
         var exchangesData = ServicesContainer.Get<ExchangesData>();
         foreach (var bank in exchangesData.BanksNames)
         {
            rates.Add(exchangesData.GetOffer("Binance", bank, currencyName, TradeType.Buy).Price);
            rates.Add(exchangesData.GetOffer("Binance", bank, currencyName, TradeType.Sell).Price);
         }
         updateRangeValues(rates, range);

      }

      private void updateRangeValues(List<object> values, string range)
      {
         var valueRange = new ValueRange();

         valueRange.Values = new List<IList<object>> { values };

         var appendRequest = _service.Spreadsheets.Values.Update(valueRange, _spreadSheetId, range);
         appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
         var appendResponde = appendRequest.Execute();
      }
   }
}
