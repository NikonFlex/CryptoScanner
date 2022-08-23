namespace CryptoParser.Models
{
   namespace Tables
   {
      [Table("MarketRates")]
      public class MarketRatesTable : ITable
      {
         private ExchangeType _exchange;

         public MarketRatesTable(ExchangeType exchange, string emptyParam)
         {
            _exchange = exchange;
         }

         public List<List<object>> CreateTable()
         {
            List<List<object>> table = new();

            table.Add(createHeaderRow());
            table.Add(createRow());

            return table;
         }

         private List<object> createHeaderRow()
         {
            string tableName = "Курсы на\nМаркете";
            var rowNames = new List<object>() { tableName };

            Constants.CurrenciesNames(_exchange).ToList().ForEach(currency => rowNames.Add(currency));
            
            return rowNames;
         }

         private List<object> createRow()
         {
            string rowName = "Курсы";
            var rates = new List<object>() { rowName };

            var exchangesData = ServicesContainer.Get<ExchangesData>();
            Constants.CurrenciesNames(_exchange).ToList().ForEach(currency => rates.Add(exchangesData.GetMarketRate(_exchange, currency).Price));
            
            return rates;
         }
      }
   }
}
