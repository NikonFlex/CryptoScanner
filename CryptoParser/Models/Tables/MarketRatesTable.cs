namespace CryptoParser.Models
{
   namespace Tables
   {
      [SimpleTable("MarketRates")]
      public class MarketRatesTable : ITable
      {
         private CVBData _cvbData;

         public MarketRatesTable(CVBType cvb, string _)
         {
            _cvbData = Constants.GetCVBData(cvb);
         }

         public List<List<object>> CreateTable(int balance, SpreadType spreadType)
         {
            List<List<object>> table = new();

            table.Add(createRow());

            return table;
         }

         private List<object> createRow()
         {
            var rates = new List<object>();

            var cvbsData = ServicesContainer.Get<CVBsData>();
            foreach (var currency in _cvbData.Currencies)
            {
               try
               {
                  var marketRate = cvbsData.GetMarketRate(_cvbData.CVB, currency).Price;
                  rates.Add(marketRate);
               }
               catch (Exception e)
               {
                  rates.Add(float.NaN);
               }
            }
            
            return rates;
         }
      }
   }
}
