namespace CryptoParser.Models
{
   namespace Tables
   {
      [Table("Convert")]
      public class ConvertTable : ITable
      {
         private ExchangeType _exchange;

         public ConvertTable(ExchangeType exchange, string emptyParam)
         {
            _exchange = exchange;
         }

         public List<List<object>> CreateTable()
         {
            List<List<object>> table = new();

            table.Add(createHeaderRow());
            Constants.CurrenciesNames(_exchange).ToList().ForEach(currency => table.Add(createRow(currency)));

            return table;
         }

         private List<object> createHeaderRow()
         {
            string tableName = "Конвертация";
            var rowNames = new List<object>() { tableName };

            Constants.CurrenciesNames(_exchange).ToList().ForEach(currency => rowNames.Add(currency));
           
            return rowNames;
         }

         private List<object> createRow(string toCurrency)
         {
            string rowName = toCurrency;
            var ratios = new List<object>() { rowName };

            var exchangesData = ServicesContainer.Get<ExchangesData>();
            foreach (var fromCurrency in Constants.CurrenciesNames(_exchange))
            {
               var fromCurrencyMarketPrice = exchangesData.GetMarketRate(_exchange, fromCurrency).Price;
               var toCurrencyMarketPrice = exchangesData.GetMarketRate(_exchange, toCurrency).Price;
               ratios.Add(Math.Round(fromCurrencyMarketPrice / toCurrencyMarketPrice, 6));
            }

            return ratios;
         }
      }
   }
}
