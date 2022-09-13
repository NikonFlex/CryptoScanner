namespace CryptoParser.Models
{
   namespace Tables
   {
      [SimpleTable("Convert")]
      public class ConvertTable : ITable
      {
         private CVBData _cvbData;

         public ConvertTable(CVBType cvb, string _)
         {
            _cvbData = Constants.GetCVBData(cvb);
         }

         public List<List<object>> CreateTable(int balance, SpreadType spreadType)
         {
            List<List<object>> table = new();

            _cvbData.Currencies.ToList().ForEach(currency => table.Add(createRow(currency)));

            return table;
         }

         private List<object> createRow(Currency toCurrency)
         {
            var ratios = new List<object>();
            var cvbsData = ServicesContainer.Get<CVBsData>();
            foreach (var fromCurrency in _cvbData.Currencies)
            {
               try
               {
                  var fromCurrencyMarketPrice = cvbsData.GetMarketRate(_cvbData.CVB, fromCurrency).Price;
                  var toCurrencyMarketPrice = cvbsData.GetMarketRate(_cvbData.CVB, toCurrency).Price;
                  ratios.Add(Math.Round(fromCurrencyMarketPrice / toCurrencyMarketPrice, 6));
               }
               catch (Exception e)
               {
                  ratios.Add($"ERROR\n{e.Message}");
               }
            }

            return ratios;
         }
      }
   }
}
