using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace CryptoParser.Models
{
   public class TablesToUpdateCollector
   {
      private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
      private readonly string _applicationName = "ExchangesInfo";
      private readonly string _spreadSheetId = "11lvjsbu-7B9Tzwin-DmMXPItDxviI7W1L5Dj9_BgKvQ";
      private List<string> _spreadSheetsToUpdateIds = new();
      private SheetsService _service;

      public IReadOnlyList<string> SpreadSheetsToUpdateIds => _spreadSheetsToUpdateIds;

      public TablesToUpdateCollector()
      {
         GoogleCredential credential;
         using (var stream = new FileStream("nikoncrypto-8b34e56c51e9.json",
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

      public void CollectTables()
      {
         _spreadSheetsToUpdateIds.Clear();
         var range = $"Таблицы для обновления!B2:B1000";
         var request = _service.Spreadsheets.Values.Get(_spreadSheetId, range);

         var response = request.Execute();
         var values = response.Values;
         if (values != null && values.Count > 0)
         {
            foreach (var row in values)
            {
               if (row != null && row[0] != "")
                  _spreadSheetsToUpdateIds.Add(row[0].ToString());
            }
         }
      }
   }
}
