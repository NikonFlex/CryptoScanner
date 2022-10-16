using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace CryptoParser
{
   namespace Models
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
            try
            {
               var cellCoord = cell.Split(':');
               Sheet = cellCoord[0];
               Column = cellCoord[1];
               Row = Int32.Parse(cellCoord[2]);
            }
            catch
            {
               throw new Exception($"Invalid cell {cell}");
            }
         }

         public string ToCellName() => $"{Column}{Row}";

         public Cell Offset(int offset)
         {
            char firstLetterInChar = char.Parse(Column);
            int firstLetterInInt = (int)firstLetterInChar;
            char lastLetterInChar;
            if (offset < 26)
            {
               int lastLetterInInt = firstLetterInInt + offset;
               lastLetterInChar = (char)lastLetterInInt;
            }
            else
            {
               int lastLetterInInt = firstLetterInInt + offset % 26;
               lastLetterInChar = (char)lastLetterInInt;
            }

            return new Cell(Sheet, lastLetterInChar.ToString(), Row);
         }
      }

      public class GoogleSheetRequestsCreator
      {
         private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
         private readonly string _applicationName = "ExchangesInfo";
         private readonly string _spreadSheetId;
         private int _balance;
         private SpreadType _spreadType;
         private Dictionary<string, Type> _simpleTables = new();
         private Dictionary<string, Type> _hardTables = new();
         private SheetsService _service;
         private List<ValueRange> _requests = new();

         public GoogleSheetRequestsCreator(GoogleTable table)
         {
            _spreadSheetId = table.ID;
            _balance = table.Balance;
            _spreadType = Utils.GetSpreadTypeFrom(table.Spread);

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

         public void UpdateSpreadSheet()
         {
            _requests.Clear();
            updateTime();
            collectTables();
            createRequests();
            updateBookValues();
            Logger.Info("finish");
         }

         private void collectTables()
         {
            //collect simple tables types
            var simpleTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                              from type in assembly.GetTypes()
                              where type.IsDefined(typeof(Tables.SimpleTableAttribute), true)
                              select type;

            foreach (var type in simpleTypes)
            {
               Tables.SimpleTableAttribute attribute = (Tables.SimpleTableAttribute)Attribute.GetCustomAttribute(type, typeof(Tables.SimpleTableAttribute));
               _simpleTables.Add(attribute.Name, type);
            }

            //collect hard tables types
            var hardTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where type.IsDefined(typeof(Tables.HardTableAttribute), true)
                            select type;

            foreach (var type in hardTypes)
            {
               Tables.HardTableAttribute attribute = (Tables.HardTableAttribute)Attribute.GetCustomAttribute(type, typeof(Tables.HardTableAttribute));
               _hardTables.Add(attribute.Name, type);
            }
         }

         private void createRequests()
         {
            createSimpleTablesRequests();
            createHardTablesRequests();
         }

         private void createSimpleTablesRequests()
         {
            foreach (var tableName in Constants.SimpleTablesRanges.Keys)
            {
               var parameters = tableName.Split('|');
               var tableType = parameters[0];
               var cvb = parameters[1];
               var tableParam = parameters[2];

               var type = _simpleTables[tableType];
               if (type == null)
                  throw new Exception($"Invalid table name {tableName}");

               var constructor = type.GetConstructor(new Type[] { typeof(CVBType), typeof(string) });
               var instance = constructor?.Invoke(new object[] { Utils.GetCVBTypeFrom(cvb), tableParam });
               var table = instance as Tables.ITable;

               Logger.Info($"start {_spreadSheetId} fill {tableName}");
               fillTable(table?.CreateTable(_balance, _spreadType), new Cell(Constants.SimpleTablesRanges[tableName]));
               Logger.Info($"finish {_spreadSheetId} fill {tableName}");
            }
         }

         private void createHardTablesRequests()
         {
            foreach (var tableName in Constants.HardTablesRanges.Keys)
            {
               var parameters = tableName.Split('|');
               var tableType = parameters[0];
               var buyCvb = parameters[1];
               var sellCvb = parameters[2];
               var tableParam1 = parameters[3];
               var tableParam2 = parameters[4];

               var type = _hardTables[tableType];
               if (type == null)
                  throw new Exception($"Invalid table name {tableName}");

               var constructor = type.GetConstructor(new Type[] { typeof(CVBType), typeof(CVBType), typeof(string), typeof(string) });
               var instance = constructor?.Invoke(new object[] { Utils.GetCVBTypeFrom(buyCvb), Utils.GetCVBTypeFrom(sellCvb), tableParam1, tableParam2 });
               var table = instance as Tables.ITable;

               Logger.Info($"start {_spreadSheetId} fill {tableName}");
               fillTable(table?.CreateTable(_balance, _spreadType), new Cell(Constants.HardTablesRanges[tableName]));
               Logger.Info($"finish {_spreadSheetId} fill {tableName}");
            }
         }

         private void updateTime() => _requests.Add(createRequest(new List<object>() { $"{TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")).ToString("H:mm:ss")} по МСК" }, createRange(new Cell(Constants.TimePos), 1)));

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
            Cell lastCell = firstCell.Offset(tableWidth);
            return $"{firstCell.Sheet}!{firstCell.ToCellName()}:{lastCell.ToCellName()}";
         }

         private void updateBookValues()
         {
            BatchUpdateValuesRequest requestBody = new BatchUpdateValuesRequest();
            requestBody.ValueInputOption = "USER_ENTERED";
            requestBody.Data = _requests;

            Logger.Info($"{_spreadSheetId} start update");
            SpreadsheetsResource.ValuesResource.BatchUpdateRequest request = _service.Spreadsheets.Values.BatchUpdate(requestBody, _spreadSheetId);
            try
            {
               _ = request.Execute();
               Logger.Info($"{_spreadSheetId} finish update");
            }
            catch
            {
               Logger.Info($"{_spreadSheetId} update failed");
            }
         }

         private ValueRange createRequest(List<object> values, string range) => new ValueRange() { Values = new List<IList<object>> { values }, Range = range };
      }
   }
}
