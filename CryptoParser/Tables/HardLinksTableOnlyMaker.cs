using CryptoParser.Models;

namespace CryptoParser
{
   namespace Tables
   {
      [HardTable("HardLinksOnlyMaker")]
      public class HardLinksTableOnlyMaker : ITable
      {
         private CVBData _sellCvbData;
         private CVBData _buyCvbData;
         private Bank _buyBank;
         private Currency _sellCurrency;
         private int _balance;
         private SpreadType _spreadType;

         public HardLinksTableOnlyMaker(CVBType sellCvb, CVBType buyCvb, string buyBank, string sellCurrency)
         {
            _sellCvbData = Constants.GetCVBData(sellCvb);
            _buyCvbData = Constants.GetCVBData(buyCvb);
            _buyBank = Utils.GetBankTypeFrom(buyBank);
            _sellCurrency = Utils.GetCurrencyTypeFrom(sellCurrency);
         }

         public List<List<object>> CreateTable(int balance, SpreadType spreadType)
         {
            _balance = balance;
            _spreadType = spreadType;

            List<List<object>> table = new();

            _sellCvbData.Banks.ToList().ForEach(sellBank => table.Add(createRow(sellBank)));

            return table;
         }

         private List<object> createRow(Bank sellBank)
         {
            var lines = new List<object>();

            var currencies = _buyCvbData.Currencies;
            foreach (var currency in currencies)
               lines.Add(Math.Round(calcLine(currency, sellBank), 2));
            
            return lines;
         }

         private float calcLine(Currency buyCurrency, Bank sellBank)
         {
            var cvbsData = ServicesContainer.Get<CVBsDataManager>().GetData();
            var sellOffer = cvbsData.GetOffer(_sellCvbData.CVB, sellBank, _sellCurrency, TradeType.Sell);
            var buyOffer = cvbsData.GetOffer(_buyCvbData.CVB, _buyBank, buyCurrency, TradeType.Buy);
            var sellCryptoMarketRate = cvbsData.GetMarketRate(_sellCvbData.CVB, _sellCurrency);
            var buyCryptoMarketRate = cvbsData.GetMarketRate(_buyCvbData.CVB, buyCurrency);

            try
            {
               float spreadWithoutCommission = (_balance / sellOffer.Price * sellCryptoMarketRate.Price / buyCryptoMarketRate.Price * buyOffer.Price - _balance);

               if (_spreadType == SpreadType.Rub)
                  return (float)(spreadWithoutCommission - spreadWithoutCommission * 0.01);
               else
                  return 100 * (float)(spreadWithoutCommission - spreadWithoutCommission * 0.01) / _balance;
            }
            catch
            {
               return float.NaN;
            }
         }
      }
   }
}
