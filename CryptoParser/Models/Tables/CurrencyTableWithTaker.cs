namespace CryptoParser.Models
{
   namespace Tables
   {
      [SimpleTable("CurrencyWithTaker")]
      public class CurrencyTableWithTaker : ITable
      {
         private CVBData _cvbData;
         private Currency _currency;
         private int _balance;
         private SpreadType _spreadType;

         public CurrencyTableWithTaker(CVBType cvb, string currency)
         {
            _cvbData = Constants.GetCVBData(cvb);
            _currency = Utils.GetCurrencyTypeFrom(currency);
         }

         public List<List<object>> CreateTable(int balance, SpreadType spreadType)
         {
            _balance = balance;
            _spreadType = spreadType;

            List<List<object>> table = new();

            _cvbData.Banks.ToList().ForEach(bank => table.Add(createRow(bank)));

            return table;
         }

         private List<object> createRow(Bank sellBank)
         {
            var spreads = new List<object>();

            var cvbsData = ServicesContainer.Get<CVBsData>();
            foreach (var buyBank in _cvbData.Banks)
            {
               try
               {
                  var buyOfferPrice = cvbsData.GetOffer(_cvbData.CVB, buyBank, _currency, TradeType.Buy).Price;
                  var sellOfferPrice = cvbsData.GetOffer(_cvbData.CVB, sellBank, _currency, TradeType.Buy).Price;

                  var spreadWithoutCommission = _balance / sellOfferPrice * buyOfferPrice - _balance;

                  if (_spreadType == SpreadType.Rub)
                     spreads.Add(Math.Round((float)(spreadWithoutCommission - spreadWithoutCommission * 0.01), 2));
                  else
                     spreads.Add(Math.Round(100 * (float)(spreadWithoutCommission - spreadWithoutCommission * 0.01) / _balance, 2));
               }
               catch (Exception e)
               {
                  spreads.Add(float.NaN);
               }
            }

            return spreads;
         }
      }
   }
}