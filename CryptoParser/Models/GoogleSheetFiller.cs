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

      public void CreateEntry(float price)
      {
         var range = $"{_sheet}!A1:B1";
         var valueRange = new ValueRange();

         var objectList = new List<object>() { price, DateTime.UtcNow };
         valueRange.Values = new List<IList<object>> { objectList };

         var appendRequest = _service.Spreadsheets.Values.Update(valueRange, _spreadSheetId, range);
         appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
         var appendResponde = appendRequest.Execute();
      }
   }
}
