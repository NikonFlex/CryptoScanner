namespace CryptoParser.Models
{
   public class ExchangesData
   {
      private List<Offer> _offers = new();
      private List<MarketRate> _marketRates = new();

      public void ClearData()
      {
         _offers.Clear();
         _marketRates.Clear();
      }

      public void AddOffer(Offer offer)
      {
         CryptoParser.Services.Logger.Info($"Add offer: bank {offer.Bank} currency {offer.Currency}");
         _offers.Add(offer);

      }
      public void AddMarketPrice(MarketRate marketPrice) => _marketRates.Add(marketPrice);
      
      public Offer? GetOffer(string exchange, string bank, string currency, TradeType tradeType)
      {
         return _offers.FirstOrDefault(offer => offer.Exchange == exchange &&
                                                offer.Bank == bank &&
                                                offer.Currency == currency &&
                                                offer.TradeType == tradeType);
      }

      public MarketRate? GetMarketRate(string exchange, string currency)
      {
         return _marketRates.FirstOrDefault(price => price.Exchange == exchange &&
                                                      price.Currency == currency);
      }
   }
}
