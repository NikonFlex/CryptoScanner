using CryptoParser.Models;

namespace CryptoParser
{
   namespace Tables
   {
      [SimpleTable("Rates")]
      public class RatesTable : ITable
      {
         private CVBData _cvbData;

         public RatesTable(CVBType cvb, string _)
         {
            _cvbData = Constants.GetCVBData(cvb);
         }

         public List<List<object>> CreateTable(int balance, SpreadType spreadType)
         {
            List<List<object>> table = new();

            _cvbData.Currencies.ToList().ForEach(currency => table.Add(createRow(currency)));
            
            return table;
         }

         private List<object> createRow(Currency currency)
         {
            var rates = new List<object>();

            var cvbsData = ServicesContainer.Get<CVBsDataManager>().GetData();
            foreach (var bank in _cvbData.Banks)
            {
               try
               {
                  var offer = cvbsData.GetOffer(_cvbData.CVB, bank, currency, TradeType.Buy).Price;
                  rates.Add(Math.Round(offer, 2));
               }
               catch (Exception e)
               {
                  rates.Add($"ERROR\n{e.Message}");
               }

               try
               {
                  var offer = cvbsData.GetOffer(_cvbData.CVB, bank, currency, TradeType.Sell).Price;
                  rates.Add(Math.Round(offer, 2));
               }
               catch (Exception e)
               {
                  rates.Add($"ERROR\n{e.Message}");
               }
            }

            return rates;
         }
      }
   }
}
