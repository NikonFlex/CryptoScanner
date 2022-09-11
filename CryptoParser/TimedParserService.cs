using CryptoParser.Models;
using System.Reflection;

namespace CryptoParser
{
   public class TimedParserService : IHostedService, IDisposable
   {
      private Timer? _timerForParcing = null;
      private Timer? _timerForTablesCollector = null;
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

         _timerForParcing = new Timer(Update, null, TimeSpan.Zero,
             TimeSpan.FromSeconds(35));

         _timerForTablesCollector = new Timer(UpdateTables, null, TimeSpan.Zero,
             TimeSpan.FromSeconds(500));

         return Task.CompletedTask;
      }

      public Task StopAsync(CancellationToken cancellationToken)
      {
         Logger.Info("Timed Hosted Service is stopping.");

         _timerForParcing?.Change(Timeout.Infinite, 0);

         return Task.CompletedTask;
      }

      private void Update(object? state)
      {
         clickYourself();
         UpdateCVBsData();
      }

      private void UpdateCVBsData()
      {
         Logger.Info("Start parse cvbs");

         ServicesContainer.Get<CVBsData>().ClearData();

         List<Task> tasks = new();
         _parsers.ForEach(parser => tasks.Add(parser.UpdateDataAsync()));
         Task.WaitAll(tasks.ToArray());

         var collector = ServicesContainer.Get<TablesToUpdateCollector>();
         foreach (var spreadSheet in collector.SpreadSheetsToUpdateIds.ToList())
         {
            var filler = new GoogleSheetRequestsCreator(spreadSheet);
            filler.UpdateSpreadSheet();
         }

         Logger.Info("cvbs parsed");
      }

      private void UpdateTables(object? state)
      {
         var collector = ServicesContainer.Get<TablesToUpdateCollector>();
         collector.CollectTables();
      }

      private void clickYourself()
      {
         HttpClient client = new();
         var requestUri = "http://cryptoscannerp2p-001-site1.atempurl.com/";
         client.GetAsync(requestUri);
      }

      public void Dispose()
      {
         _timerForParcing?.Dispose();
         _timerForTablesCollector?.Dispose();
      }
   }
}
