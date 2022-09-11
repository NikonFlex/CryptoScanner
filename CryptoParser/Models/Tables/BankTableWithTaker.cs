namespace CryptoParser.Models
{
   namespace Tables
   {
      [SimpleTable("BankWithTaker")]
      public class BankTableWithMaker : ITable
      {
         private CVBData _cvbData;
         private string _bank;
         private int _balance;
         private SpreadType _spreadType;

         public BankTableWithMaker(CVBType cvb, string bank)
         {
            _cvbData = Constants.GetCVBData(cvb);
            _bank = bank;
         }

         public List<List<object>> CreateTable(int balance, SpreadType spreadType)
         {
            _balance = balance;
            _spreadType = spreadType;

            List<List<object>> table = new();

            _cvbData.Currencies.ToList().ForEach(currency => table.Add(createRow(currency)));

            return table;
         }

         private List<object> createRow(string sellCurrency)
         {
            var lines = new List<object>();

            foreach (var currency in _cvbData.Currencies)
               lines.Add(Math.Round(calcLine(currency, sellCurrency), 2));
            
            return lines;
         }

         private float calcLine(string buyCurrency, string sellCurrency)
         {
            var cvbsData = ServicesContainer.Get<CVBsData>();
            var sellOffer = cvbsData.GetOffer(_cvbData.CVB, _bank, sellCurrency, TradeType.Buy);
            var buyOffer = cvbsData.GetOffer(_cvbData.CVB, _bank, buyCurrency, TradeType.Buy);
            var sellCryptoMarketRate = cvbsData.GetMarketRate(_cvbData.CVB, sellCurrency);
            var buyCryptoMarketRate = cvbsData.GetMarketRate(_cvbData.CVB, buyCurrency);

            try
            {
               float spreadWithoutCommission = _balance / sellOffer.Price * sellCryptoMarketRate.Price / buyCryptoMarketRate.Price * buyOffer.Price - _balance;

               if (_spreadType == SpreadType.Rub)
                  return (float)(spreadWithoutCommission - spreadWithoutCommission * 0.1);
               else
                  return (100 * (float)(spreadWithoutCommission - spreadWithoutCommission * 0.1) / _balance);
            }
            catch
            {
               return float.NaN;
            }
         }
      }
   }
}
