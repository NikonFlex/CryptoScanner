namespace CryptoParser.Models
{
   namespace Tables
   {
      [Table("BankLinks")]
      public class BankLinksTable : ITable
      {
         private ExchangeType _exchange;
         private string _buyBank;

         public BankLinksTable(ExchangeType exchange, string buyBank)
         {
            _exchange = exchange;
            _buyBank = buyBank;
         }

         public List<List<object>> CreateTable()
         {
            List<List<object>> table = new();

            table.Add(createHeaderRow());
            foreach (var sellCurrency in Constants.CurrenciesNames(_exchange))
               Constants.BanksNames(_exchange).ToList().ForEach(sellBank => table.Add(createRow(sellCurrency, sellBank)));

            return table;
         }

         private List<object> createHeaderRow()
         {
            string tableName = Constants.EngToRusDict[_buyBank];
            var rowNames = new List<object>() { tableName };
            Constants.CurrenciesNames(_exchange).ToList().ForEach(currency => rowNames.Add($"Покупка\n{currency}"));

            return rowNames;
         }

         private List<object> createRow(string sellCurrency, string sellBank)
         {
            string rowName = $"Продажа {sellCurrency}\nна {Constants.EngToRusDict[sellBank]}";
            var lines = new List<object>() { rowName };

            Constants.CurrenciesNames(_exchange).ToList().ForEach(currency => lines.Add(calcLine(currency, sellBank, sellCurrency)));

            return lines;
         }

         private string calcLine(string buyCurrency, string sellBank, string sellCurrency)
         {
            var exchangesData = ServicesContainer.Get<ExchangesData>();
            var sellOffer = exchangesData.GetOffer(_exchange, sellBank, sellCurrency, TradeType.Sell);
            var buyOffer = exchangesData.GetOffer(_exchange, _buyBank, buyCurrency, TradeType.Buy);
            var sellCryptoMarketRate = exchangesData.GetMarketRate(_exchange, sellCurrency);
            var buyCryptoMarketRate = exchangesData.GetMarketRate(_exchange, buyCurrency);

            var spread = (Constants.Balance / sellOffer.Price *
                   sellCryptoMarketRate.Price / buyCryptoMarketRate.Price
                   * buyOffer.Price - Constants.Balance);

            //return spread - (float)(spread * 0.1);
            //debug
            return $"{spread}={Constants.Balance}/{sellOffer.Price}*\n{sellCryptoMarketRate.Price}/{buyCryptoMarketRate.Price}*{buyOffer.Price}";
         }
      }
   }
}
