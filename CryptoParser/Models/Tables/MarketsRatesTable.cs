namespace CryptoParser.Models
{
   namespace Tables
   {
      public static class MarketsRatesTable
      {
         public static List<List<object>> Create()
         {
            List<List<object>> table = new();

            table.Add(createHeaderRow());
            table.Add(createRow());

            return table;
         }

         private static List<object> createHeaderRow()
         {
            string tableName = "Курсы на\nМаркете";
            var rowNames = new List<object>() { tableName };

            for (int i = 1; i < Constants.CurrenciesNames.Length; i++)
               rowNames.Add(Constants.CurrenciesNames[i]);

            return rowNames;
         }

         private static List<object> createRow()
         {
            string rowName = "Курсы";
            var rates = new List<object>() { rowName };

            var exchangesData = Services.ServicesContainer.Get<ExchangesData>();
            for (int i = 1; i < Constants.CurrenciesNames.Length; i++)
               rates.Add(exchangesData.GetMarketRate("Binance", Constants.CurrenciesNames[i]).Price);

            return rates;
         }
      }
   }
}
