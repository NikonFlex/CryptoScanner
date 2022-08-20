namespace CryptoParser.Models
{
   public static class Constants
   {
      public static readonly string[] BanksNames = { "Tinkoff", "RosBank", "RaiffeisenBankRussia", "QIWI", "YandexMoneyNew" };
      public static readonly string[] CurrenciesNames = { "USDT", "BTC", "BUSD", "BNB", "ETH" };
      public static readonly Dictionary<string, string> EngToRusDict = new()
      {
         { "Tinkoff", "Тинькофф" },
         { "RosBank", "Росбанк" },
         { "RaiffeisenBankRussia", "Райфайзен" },
         { "QIWI", "QIWI" },
         { "YandexMoneyNew", "ЮMoney" },
      };
      public static readonly int Balance = 100000;
   }
}
