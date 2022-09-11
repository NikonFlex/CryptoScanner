namespace ScannerClicker
{
   public class TimedParserService : IHostedService, IDisposable
   {
      private Timer? _timerForClicker = null;

      public TimedParserService() { }
      

      public Task StartAsync(CancellationToken cancellationToken)
      {
         _timerForClicker = new Timer(Update, null, TimeSpan.Zero,
             TimeSpan.FromSeconds(60));

         return Task.CompletedTask;
      }

      public Task StopAsync(CancellationToken cancellationToken)
      {
         _timerForClicker?.Change(Timeout.Infinite, 0);
         return Task.CompletedTask;
      }

      private void Update(object? state)
      {
         HttpClient client = new();
         var requestUri = "http://cryptoscannerp2p-001-site1.atempurl.com/";
         client.GetAsync(requestUri);
      }

      public void Dispose()
      {
         _timerForClicker?.Dispose();
      }
   }
}
