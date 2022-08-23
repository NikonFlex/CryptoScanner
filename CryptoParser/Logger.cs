namespace CryptoParser
{
   public class Logger
   {
      private static readonly Logger _instance = new Logger();
      private static log4net.ILog _log;

      public static Logger Instance => _instance;

      public static void Info(string message) => Instance.Log.Info(message);
      public static void Warn(string message) => Instance.Log.Warn(message);
      public static void Error(string message) => Instance.Log.Error(message);
      private Logger()
      {
         _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      }

      private log4net.ILog Log => _log;
   }
}
