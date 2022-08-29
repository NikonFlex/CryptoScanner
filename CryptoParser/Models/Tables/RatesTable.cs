namespace CryptoParser.Models
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

         private List<object> createRow(string currency)
         {
            var rates = new List<object>();

            var cvbsData = ServicesContainer.Get<CVBsData>();
            foreach (var bank in _cvbData.Banks)
            {
               try
               {
                  rates.Add(Math.Round(cvbsData.GetOffer(_cvbData.CVB, bank, currency, TradeType.Buy).Price, 2));
               }
               catch (Exception e)
               {
                  rates.Add($"ERROR\n{e.Message}");
               }

               try
               {
                  rates.Add(Math.Round(cvbsData.GetOffer(_cvbData.CVB, bank, currency, TradeType.Sell).Price, 2));

                  if (bank == "RaiffeisenBankRussia" || bank == "Raiffaizen")
                     rates.Add("");
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
