namespace CryptoParser.Services
{
   public class Logger
   {
      private readonly log4net.ILog _log;

      public Logger()
      {
         _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      }

      public log4net.ILog Log => _log;
   }
}
