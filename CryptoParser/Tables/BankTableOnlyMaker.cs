using CryptoParser.Models;

namespace CryptoParser
{
   namespace Tables
   {
      [SimpleTable("BankOnlyMaker")]
      public class BankTableOnlyMaker : ITable
      {
         private CVBData _cvbData;
         private Bank _bank;
         private int _balance;
         private SpreadType _spreadType;

         public BankTableOnlyMaker(CVBType cvb, string bank)
         {
            _cvbData = Constants.GetCVBData(cvb);
            _bank = Utils.GetBankTypeFrom(bank);
         }

         public List<List<object>> CreateTable(int balance, SpreadType spreadType)
         {
            _balance = balance;
            _spreadType = spreadType;

            List<List<object>> table = new();

            _cvbData.Currencies.ToList().ForEach(currency => table.Add(createRow(currency)));

            return table;
         }

         private List<object> createRow(Currency sellCurrency)
         {
            var lines = new List<object>();

            foreach (var currency in _cvbData.Currencies)
            {
               lines.Add(Math.Round(calcLine(currency, sellCurrency), 2));
            }

            return lines;
         }

         private float calcLine(Currency buyCurrency, Currency sellCurrency)
         {
            var cvbsData = ServicesContainer.Get<CVBsDataManager>().GetData();
            var sellOffer = cvbsData.GetOffer(_cvbData.CVB, _bank, sellCurrency, TradeType.Sell);
            var buyOffer = cvbsData.GetOffer(_cvbData.CVB, _bank, buyCurrency, TradeType.Buy);
            var sellCryptoMarketRate = cvbsData.GetMarketRate(_cvbData.CVB, sellCurrency);
            var buyCryptoMarketRate = cvbsData.GetMarketRate(_cvbData.CVB, buyCurrency);

            try
            {
               float spreadWithoutCommission = _balance / sellOffer.Price * sellCryptoMarketRate.Price / buyCryptoMarketRate.Price * buyOffer.Price - _balance;
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
