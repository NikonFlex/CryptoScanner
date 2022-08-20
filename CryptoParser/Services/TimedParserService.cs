using CryptoParser.Models;

namespace CryptoParser.Services
{
   public class TimedParserService : IHostedService, IDisposable
   {
      private Timer? _timer = null;

      public TimedParserService() { }

      public Task StartAsync(CancellationToken cancellationToken)
      {
         Logger.Instance.Log.Info("Timed Hosted Service running.");

         _timer = new Timer(ParseExchanges, null, TimeSpan.Zero,
             TimeSpan.FromSeconds(60));

         return Task.CompletedTask;
      }

      public Task StopAsync(CancellationToken cancellationToken)
      {
         Logger.Instance.Log.Info("Timed Hosted Service is stopping.");

         _timer?.Change(Timeout.Infinite, 0);

         return Task.CompletedTask;
      }

      private void ParseExchanges(object? state)
      {
         Logger.Instance.Log.Info("Start parse exchanges");

         ServicesContainer.Get<ExchangesData>().ClearData();

         Models.Parsers.BinanceParser.UpdateDataAsync().ContinueWith(res =>
            ServicesContainer.Get<GoogleSheetFiller>().UpdateSheet()
            );
      }

      public void Dispose()
      {
         _timer?.Dispose();
      }
   }
}
