namespace CryptoParser.Models
{
   namespace Tables
   {
      [HardTable("HardLinksWithTaker")]
      public class HardLinksTableWithTaker : ITable
      {
         private CVBData _sellCvbData;
         private CVBData _buyCvbData;
         private string _buyBank;
         private string _sellCurrency;
         private int _balance;
         private SpreadType _spreadType;

         public HardLinksTableWithTaker(CVBType sellCvb, CVBType buyCvb, string buyBank, string sellCurrency)
         {
            _sellCvbData = Constants.GetCVBData(sellCvb);
            _buyCvbData = Constants.GetCVBData(buyCvb);
            _buyBank = buyBank;
            _sellCurrency = sellCurrency;
         }

         public List<List<object>> CreateTable(int balance, SpreadType spreadType)
         {
            _balance = balance;
            _spreadType = spreadType;

            List<List<object>> table = new();

            _sellCvbData.Banks.ToList().ForEach(sellBank => table.Add(createRow(sellBank)));

            return table;
         }

         private List<object> createRow(string sellBank)
         {
            var lines = new List<object>();

            var currencies = _buyCvbData.Currencies;
            foreach (var currency in currencies)
            {
               try
               {
                  lines.Add(Math.Round(calcLine(currency, sellBank), 2));
               }
               catch (Exception e)
               {
                  lines.Add($"ERROR\n{e.Message}");
               }
            }

            return lines;
         }

         private float calcLine(string buyCurrency, string sellBank)
         {
            var cvbsData = ServicesContainer.Get<CVBsData>();
            var sellOffer = cvbsData.GetOffer(_sellCvbData.CVB, sellBank, _sellCurrency, TradeType.Buy);
            var buyOffer = cvbsData.GetOffer(_buyCvbData.CVB, _buyBank, buyCurrency, TradeType.Buy);
            var sellCryptoMarketRate = cvbsData.GetMarketRate(_sellCvbData.CVB, _sellCurrency);
            var buyCryptoMarketRate = cvbsData.GetMarketRate(_buyCvbData.CVB, buyCurrency);

            try
            {
               float spreadWithoutCommission = (_balance / sellOffer.Price * sellCryptoMarketRate.Price / buyCryptoMarketRate.Price * buyOffer.Price - _balance);

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
