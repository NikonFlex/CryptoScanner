using CryptoParser.Models;
using System.Reflection;

namespace CryptoParser
{
   public class TimedParserService : IHostedService, IDisposable
   {
      private Timer? _timer = null;
      private List<Models.Parsers.IParser> _parsers = new();

      public TimedParserService()
      {
         _parsers.AddRange(from type in Assembly.GetExecutingAssembly().GetTypes()
                           where type.IsDefined(typeof(Models.Parsers.ParserAttribute), true)
                           let constructor = type.GetConstructor(Type.EmptyTypes)
                           let instance = constructor?.Invoke(Type.EmptyTypes)
                           where instance != null
                           select (Models.Parsers.IParser)instance);
      }

      public Task StartAsync(CancellationToken cancellationToken)
      {
         Logger.Info("Timed Hosted Service running.");

         _timer = new Timer(ParseCVBs, null, TimeSpan.Zero,
             TimeSpan.FromSeconds(60));

         return Task.CompletedTask;
      }

      public Task StopAsync(CancellationToken cancellationToken)
      {
         Logger.Info("Timed Hosted Service is stopping.");

         _timer?.Change(Timeout.Infinite, 0);

         return Task.CompletedTask;
      }

      private void ParseCVBs(object? state)
      {
         clickYourself();

         Logger.Info("Start parse cvbs");

         ServicesContainer.Get<CVBsData>().ClearData();

         List<Task> tasks = new();
         _parsers.ForEach(parser => tasks.Add(parser.UpdateDataAsync()));
         Task.WaitAll(tasks.ToArray());

         var collector = ServicesContainer.Get<TablesToUpdateCollector>();
         collector.CollectTables();

         //string[] spreadSheets = { "1aFMw6hTxMkl_30MuPVp3Nb8yc32bpeHPcAjrDeKpabc" };
         foreach (var spreadSheet in collector.SpreadSheetsToUpdateIds)
         {
            var filler = new GoogleSheetFiller(spreadSheet);
            filler.UpdateSpreadSheet();
         }
      }

      private void clickYourself()
      {
         HttpClient client = new();
         var requestUri = "http://nikontest-001-site1.dtempurl.com/";
         client.GetAsync(requestUri);
      }

      public void Dispose()
      {
         _timer?.Dispose();
      }
   }
}
