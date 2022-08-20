namespace CryptoParser.Models
{
   namespace Tables 
   { 
      public static class RatesTable
      {
         public static List<List<object>> Create()
         {
            List<List<object>> table = new();

            table.Add(createHeaderRow());

            foreach (var currencyName in Constants.CurrenciesNames)
            {
               table.Add(createRow(currencyName));
            }

            return table;
         }

         private static List<object> createHeaderRow()
         {
            string tableName = "RUB Курсы:";
            var rowNames = new List<object>() { tableName };

            foreach (var bank in Constants.BanksNames)
            {
               rowNames.Add("Продажа" + "\n" + $"{Constants.EngToRusDict[$"{bank}"]}");
               rowNames.Add("Покупка" + "\n" + $"{Constants.EngToRusDict[$"{bank}"]}");
            }

            return rowNames;
         }

         private static List<object> createRow(string currencyName)
         {
            string rowName = currencyName;
            var rates = new List<object>() { rowName };

            var exchangesData = CryptoParser.Services.ServicesContainer.Get<ExchangesData>();
            foreach (var bank in Constants.BanksNames)
            {
               rates.Add(exchangesData.GetOffer("Binance", bank, currencyName, TradeType.Buy).Price);
               rates.Add(exchangesData.GetOffer("Binance", bank, currencyName, TradeType.Sell).Price);
            }

            return rates;
         }
      }
   }
}
