using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace CryptoParser.Models
{
   public class Cell
   {
      public string Sheet { get; private set; }
      public string Column { get; private set; }
      public int Row { get; private set; }

      public Cell(string sheet, string column, int row)
      {
         Sheet = sheet;
         Column = column;
         Row = row;
      }

      public Cell(string cell)
      {
         var cellCoord = cell.Split(':');
         Sheet = cellCoord[0];
         Column = cellCoord[1];
         Row = Int32.Parse(cellCoord[2]);
      }

      public string ToCellName() => $"{Column}{Row}";
   }

   public class GoogleSheetFiller
   {
      private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
      private readonly string _applicationName = "ExchangesInfo";
      private readonly string _spreadSheetId = "1hr3GQofkjQ62P1k4hwX1N2CpOWJpu8wqiE4Kx-IT8MM";
      private Dictionary<string, Type> _allTables = new();
      private SheetsService _service;
      private List<ValueRange> _requests = new();

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

         var types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                     from type in assembly.GetTypes()
                     where type.IsDefined(typeof(Tables.TableAttribute), true)
                     select type;

         foreach (var type in types)
         {
            Tables.TableAttribute attribute = (Tables.TableAttribute)Attribute.GetCustomAttribute(type, typeof(Tables.TableAttribute));
            _allTables.Add(attribute.Name, type);
         }
      }

      public void UpdateSheet()
      {
         _requests.Clear();
         createRequests();
         updateBookValues();
      }

      private void createRequests()
      {
         updateTime(new Cell("Binance1", "A", 1));

         foreach (var tableName in Constants.TablesPos.Keys)
         {
            var parameters = tableName.Split('|');
            var tableType = parameters[0];
            var exchange = parameters[1];
            var tableParam = parameters[2];

            var type = _allTables[tableType];
            var constructor = type.GetConstructor(new Type[] { typeof(ExchangeType), typeof(string) });
            var instance = constructor?.Invoke(new object[] { Utils.GetExchangeTypeFrom(exchange), tableParam });
            var table = instance as Tables.ITable;

            fillTable(table?.CreateTable(), new Cell(Constants.TablesPos[tableName]));
         }
      }

      private void updateTime(Cell timeCell) => _requests.Add(createRequest(new List<object>() { "Update:", DateTime.Now.ToString() }, createRange(timeCell, 1)));

      private void fillTable(List<List<object>> table, Cell topleft)
      {
         int currentRow = topleft.Row;
         foreach (var row in table)
         {
            _requests.Add(createRequest(row, createRange(new Cell(topleft.Sheet, topleft.Column, currentRow), row.Count)));
            currentRow++;
         }
      }

      private string createRange(Cell firstCell, int tableWidth)
      {
         char firstLetterInChar = char.Parse(firstCell.Column);
         int firstLetterInInt = (int)firstLetterInChar;
         int lastLetterInInt = firstLetterInInt + tableWidth;
         char lastLetterInChar = (char)lastLetterInInt;
         Cell lastCell = new(firstCell.Sheet, lastLetterInChar.ToString(), firstCell.Row);
         return $"{firstCell.Sheet}!{firstCell.ToCellName()}:{lastCell.ToCellName()}";
      }

      private void updateBookValues()
      {
         BatchUpdateValuesRequest requestBody = new BatchUpdateValuesRequest();
         requestBody.ValueInputOption = "USER_ENTERED";
         requestBody.Data = _requests;

         SpreadsheetsResource.ValuesResource.BatchUpdateRequest request = _service.Spreadsheets.Values.BatchUpdate(requestBody, _spreadSheetId);
         _ = request.Execute();
      }

      private ValueRange createRequest(List<object> values, string range) => new ValueRange() { Values = new List<IList<object>> { values }, Range = range };
   }
}
