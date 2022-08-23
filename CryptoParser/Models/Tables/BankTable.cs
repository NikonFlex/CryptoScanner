namespace CryptoParser.Models
{
   namespace Tables
   {
      [Table("Bank")]
      public class BankTable : ITable
      {
         private ExchangeType _exchange;
         private string _bank;

         public BankTable(ExchangeType exchange, string bank)
         {
            _exchange = exchange;
            _bank = bank;
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
            string tableName = Constants.EngToRusDict[_bank];
            var rowNames = new List<object>() { tableName };
            Constants.CurrenciesNames(_exchange).ToList().ForEach(currency => rowNames.Add($"Покупка\n{currency}"));

            return rowNames;
         }

         private List<object> createRow(string sellCurrency)
         {
            string rowName = $"Продажа\n{sellCurrency}";
            var lines = new List<object>() { rowName };

            Constants.CurrenciesNames(_exchange).ToList().ForEach(currency => lines.Add(calcLine(currency, sellCurrency)));

            return lines;
         }

         private string calcLine(string buyCurrency, string sellCurrency)
         {
            var exchangesData = ServicesContainer.Get<ExchangesData>();
            var sellOffer = exchangesData.GetOffer(_exchange, _bank, sellCurrency, TradeType.Sell);
            var buyOffer = exchangesData.GetOffer(_exchange, _bank, buyCurrency, TradeType.Buy);
            var sellCryptoMarketRate = exchangesData.GetMarketRate(_exchange, sellCurrency);
            var buyCryptoMarketRate = exchangesData.GetMarketRate(_exchange, buyCurrency);

            var spread = (Constants.Balance / sellOffer.Price *
                   sellCryptoMarketRate.Price / buyCryptoMarketRate.Price
                   * buyOffer.Price - Constants.Balance);

            //return spread - (float)(spread * 0.1);
            // debug
            return $"{spread}={Constants.Balance}/{sellOffer.Price}*\n{sellCryptoMarketRate.Price}/{buyCryptoMarketRate.Price}*{buyOffer.Price}";
         }
      }
   }
}
