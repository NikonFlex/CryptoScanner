namespace CryptoParser.Services
{
   public class Logger
   {
      private static readonly Logger _instance = new Logger();
      private static log4net.ILog _log;

      public static Logger Instance => _instance;

      private Logger()
      {
         _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      }

      public log4net.ILog Log => _log;
   }
}
