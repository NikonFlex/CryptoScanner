using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

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
      }

      private void updateRatesTable()
      {
         createHeaderRow();

         int rowCounter = 2;
         foreach (var currencyName in Binance.CurrenciesNames)
         {
            createCurrencyRow(currencyName, rowCounter);
            rowCounter++;
         }
      }

      private void createHeaderRow()
      {
         string tableName = "RUB Курсы:";

         var range = $"{_sheet}!A1:L1";
         var rowNames = new List<object>() { tableName };
         foreach (var bank in Binance.BanksNames)
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
         foreach (var bank in Binance.Banks)
         {
            foreach (var currency in bank.Currencies)
            {
               if (currency.Name == currencyName)
               {
                  rates.Add(currency.BuyPrice);
                  rates.Add(currency.SellPrice);
                  break;
               }
            }
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
