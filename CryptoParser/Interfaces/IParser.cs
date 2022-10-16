namespace CryptoParser
{
   namespace Parsing
   {
      public interface IParser
      {
         public Task<List<Models.Offer>> ParseOffersAsync();
         public Task<List<Models.MarketRate>> ParseMarketRatesAsync();
      }
   }
}
