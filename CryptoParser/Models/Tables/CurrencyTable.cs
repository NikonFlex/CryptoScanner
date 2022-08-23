namespace CryptoParser.Models
{
   namespace Tables
   {
      [Table("Currency")]
      public class CurrencyTable : ITable
      {
         private ExchangeType _exchange;
         private string _currency;

         public CurrencyTable(ExchangeType exchange, string currency)
         {
            _exchange = exchange;
            _currency = currency;
         }

         public List<List<object>> CreateTable()
         {
            List<List<object>> table = new();

            table.Add(createHeaderRow());
            Constants.BanksNames(_exchange).ToList().ForEach(bank => table.Add(createRow(bank)));
            
            return table;
         }

         private List<object> createHeaderRow()
         {
            string tableName = _currency;
            var rowNames = new List<object>() { tableName };

            Constants.BanksNames(_exchange).ToList().ForEach(bank => rowNames.Add("Покупка" + "\n" + $"{Constants.EngToRusDict[$"{bank}"]}"));
            
            return rowNames;
         }

         private List<object> createRow(string sellBank)
         {
            string rowName = "Продажа" + "\n" + $"{Constants.EngToRusDict[$"{sellBank}"]}";
            var spreads = new List<object>() { rowName };

            var exchangesData = ServicesContainer.Get<ExchangesData>();
            foreach (var buyBank in Constants.BanksNames(_exchange))
            {
               var buyOfferPrice = exchangesData.GetOffer(_exchange, buyBank, _currency, TradeType.Buy).Price;
               var sellOfferPrice = exchangesData.GetOffer(_exchange, sellBank, _currency, TradeType.Sell).Price;

               var spread = Constants.Balance / sellOfferPrice * buyOfferPrice - Constants.Balance;
               spreads.Add(Math.Round(spread - spread * 0.1, 2));
            }

            return spreads;
         }
      }
   }
}
