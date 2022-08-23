namespace CryptoParser.Models
{
   namespace Tables
   {
      [Table("Rates")]
      public class RatesTable : ITable
      {
         private ExchangeType _exchange;

         public RatesTable(ExchangeType exchange, string emptyParam)
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
            string tableName = "RUB Курсы:";
            var rowNames = new List<object>() { tableName };

            foreach (var bank in Constants.BanksNames(_exchange))
            {
               rowNames.Add("Покупка" + "\n" + $"{Constants.EngToRusDict[$"{bank}"]}");
               rowNames.Add("Продажа" + "\n" + $"{Constants.EngToRusDict[$"{bank}"]}");
            }

            return rowNames;
         }

         private List<object> createRow(string currency)
         {
            string rowName = currency;
            var rates = new List<object>() { rowName };

            var exchangesData = ServicesContainer.Get<ExchangesData>();
            foreach (var bank in Constants.BanksNames(_exchange))
            {
               rates.Add(Math.Round(exchangesData.GetOffer(_exchange, bank, currency, TradeType.Buy).Price, 2));
               rates.Add(Math.Round(exchangesData.GetOffer(_exchange, bank, currency, TradeType.Sell).Price, 2));
            }

            return rates;
         }
      }
   }
}
