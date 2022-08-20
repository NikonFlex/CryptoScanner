namespace CryptoParser.Models
{
   namespace Tables
   {
      public static class CurrencyTable
      {
         public static List<List<object>> Create(string currencyName)
         {
            List<List<object>> table = new();

            table.Add(createHeaderRow(currencyName));

            foreach (var sellBank in Constants.BanksNames)
            {
               table.Add(createRow(currencyName, sellBank));
            }

            return table;
         }

         private static List<object> createHeaderRow(string currencyName)
         {
            string tableName = currencyName;
            var rowNames = new List<object>() { tableName };

            foreach (var bank in Constants.BanksNames)
               rowNames.Add("Покупка" + "\n" + $"{Constants.EngToRusDict[$"{bank}"]}");

            return rowNames;
         }

         private static List<object> createRow(string currencyName, string sellBank)
         {
            string rowName = "Продажа" + "\n" + $"{Constants.EngToRusDict[$"{sellBank}"]}";
            var spreads = new List<object>() { rowName };

            var exchangesData = Services.ServicesContainer.Get<ExchangesData>();
            foreach (var buyBank in Constants.BanksNames)
            {
               var buyOfferPrice = exchangesData.GetOffer("Binance", buyBank, currencyName, TradeType.Buy).Price;
               var sellOfferPrice = exchangesData.GetOffer("Binance", sellBank, currencyName, TradeType.Sell).Price;

               spreads.Add(Constants.Balance / sellOfferPrice * buyOfferPrice - Constants.Balance);
            }

            return spreads;
         }
      }
   }
}
