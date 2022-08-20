using CryptoParser.Models.ExchangesModels;

namespace CryptoParser.Services
{
   public class TimedParserService : IHostedService, IDisposable
   {
      private Timer? _timer = null;

      public TimedParserService() { }

      public Task StartAsync(CancellationToken cancellationToken)
      {
         ServicesContainer.Get<Logger>().Log.Info("Timed Hosted Service running.");

         _timer = new Timer(ParseExchanges, null, TimeSpan.Zero,
             TimeSpan.FromSeconds(60));

         return Task.CompletedTask;
      }

      public Task StopAsync(CancellationToken cancellationToken)
      {
         ServicesContainer.Get<Logger>().Log.Info("Timed Hosted Service is stopping.");

         _timer?.Change(Timeout.Infinite, 0);

         return Task.CompletedTask;
      }

      private void ParseExchanges(object? state)
      {
         ServicesContainer.Get<Logger>().Log.Info("Start parse exchanges");

         ServicesContainer.Get<ExchangesData>().UpdateDataAsync().ContinueWith(res =>
            ServicesContainer.Get<Models.GoogleSheetFiller>().UpdateSheet()
            );
         //Binance.ParseCoursesAsync().ContinueWith(res =>
         //   ServicesContainer.Get<Models.GoogleSheetFiller>().UpdateSheet()
         //   );
      }

      //private async Task<float> ParseBinance()
      //{
      //   //ServicesContainer.Get<Logger>().Log.Info("Start parse binance");
      //   //ServicesContainer.Get<ParserManager>().AddExchange(new Models.Exchange("Binance"));

      //   //return await Models.Binance.ParseCourses();
      //}

      private void setNumber(float n)
      {
         //ServicesContainer.Get<Logger>().Log.Info("Set Binance Price");
         //ServicesContainer.Get<ParserManager>().SetNumber(n);
         //ServicesContainer.Get<Logger>().Log.Info(n);
         //ServicesContainer.Get<Models.GoogleSheetFiller>().CreateEntry(n);
      }

      public void Dispose()
      {
         _timer?.Dispose();
      }
   }
}
