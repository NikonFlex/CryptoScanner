namespace CryptoParser.Models
{
   namespace Tables
   {
      [SimpleTable("Currency")]
      public class CurrencyTable : ITable
      {
         private CVBData _cvbData;
         private string _currency;
         private int _balance;
         private SpreadType _spreadType;

         public CurrencyTable(CVBType cvb, string currency)
         {
            _cvbData = Constants.GetCVBData(cvb);
            _currency = currency;
         }

         public List<List<object>> CreateTable(int balance, SpreadType spreadType)
         {
            _balance = balance;
            _spreadType = spreadType;

            List<List<object>> table = new();

            _cvbData.Banks.ToList().ForEach(bank => table.Add(createRow(bank)));
            
            return table;
         }
         
         private List<object> createRow(string sellBank)
         {
            var spreads = new List<object>();

            var cvbsData = ServicesContainer.Get<CVBsData>();
            foreach (var buyBank in _cvbData.Banks)
            {
               try
               {
                  var buyOfferPrice = cvbsData.GetOffer(_cvbData.CVB, buyBank, _currency, TradeType.Buy).Price;
                  var sellOfferPrice = cvbsData.GetOffer(_cvbData.CVB, sellBank, _currency, TradeType.Sell).Price;

                  var spreadWithoutCommission = _balance / sellOfferPrice * buyOfferPrice - _balance;

                  if (_spreadType == SpreadType.Rub)
                     spreads.Add(Math.Round((float)(spreadWithoutCommission - spreadWithoutCommission * 0.1), 2));
                  else
                     spreads.Add(Math.Round(100 * (float)(spreadWithoutCommission - spreadWithoutCommission * 0.1) / _balance, 2));
               }
               catch (Exception e)
               {
                  spreads.Add($"ERROR\n{e.Message}");
               }
            }

            return spreads;
         }
      }
   }
}
