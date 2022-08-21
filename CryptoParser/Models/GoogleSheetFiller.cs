using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using CryptoParser.Services;

namespace CryptoParser.Models
{
   public struct Cell
   {
      public string Letter;
      public int Number;
   }

   public class GoogleSheetFiller
   {
      private static readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
      private static readonly string _applicationName = "ExchangesInfo";
      private static readonly string _spreadSheetId = "1hr3GQofkjQ62P1k4hwX1N2CpOWJpu8wqiE4Kx-IT8MM";
      private static readonly string _sheet = "Binance";
      private static SheetsService _service;

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
         updateTime(new Cell() { Letter = "A", Number = 1 });

         var ratesTableFirstCell = new Cell() { Letter = "A", Number = 3 };
         fillTable(Tables.RatesTable.Create(), ratesTableFirstCell);

         var currencyTableFirstCell = new Cell() { Letter = "A", Number = 10 };
         fillTable(Tables.CurrencyTable.Create(Constants.CurrenciesNames[0]), currencyTableFirstCell);

         var marketTableFirstCell = new Cell() { Letter = "A", Number = 17 };
         fillTable(Tables.MarketRatesTable.Create(), marketTableFirstCell);
      }

      private void updateTime(Cell timeCell)
      {
         var range = createRange(timeCell, 1);
         updateRangeValues(new List<object>() { "Update:", DateTime.Now.ToString() }, range);
      }           

      private void fillTable(List<List<object>> table, Cell topleft)
      {
         int currentRow = topleft.Number;
         foreach (var row in table)
         {
            string range = createRange(new Cell() { Letter = topleft.Letter, Number = currentRow }, row.Count);
            updateRangeValues(row, range);
            currentRow++;
         }
      }

      private string createRange(Cell firstCell, int tableWidth)
      {
         char firstLetterInChar = char.Parse(firstCell.Letter);
         int firstLetterInInt = (int)firstLetterInChar;
         int lastLetterInInt = firstLetterInInt + tableWidth;
         char lastLetterInChar = (char)lastLetterInInt;
         Cell lastCell = new() { Letter = lastLetterInChar.ToString(), Number = firstCell.Number };
         return $"{_sheet}!{firstCell.Letter}{firstCell.Number}:{lastCell.Letter}{lastCell.Number}";
      }

      private void updateRangeValues(List<object> values, string range)
      {
         var valueRange = new ValueRange();
         valueRange.Values = new List<IList<object>> { values };

         var appendRequest = _service.Spreadsheets.Values.Update(valueRange, _spreadSheetId, range);
         appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
         appendRequest.Execute();
      }
   }
}
