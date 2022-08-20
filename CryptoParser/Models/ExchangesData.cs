namespace CryptoParser.Models
{
   public class ExchangesData
   {
      private List<Offer> _offers = new();
      private List<MarketRates> _marketPrices = new();

      public void ClearData()
      {
         _offers.Clear();
         _marketPrices.Clear();
      }

      public void AddOffer(Offer offer) => _offers.Add(offer);
      public void AddMarketPrice(MarketRates marketPrice) => _marketPrices.Add(marketPrice);
      
      public Offer? GetOffer(string exchange, string bank, string currency, TradeType tradeType)
      {
         return _offers.FirstOrDefault(offer => offer.Exchange == exchange &&
                                                offer.Bank == bank &&
                                                offer.Currency == currency &&
                                                offer.TradeType == tradeType);
      }

      public MarketRates? GetMarketRate(string exchange, string currency)
      {
         return _marketPrices.FirstOrDefault(price => price.Exchange == exchange &&
                                                      price.Currency == currency);
      }
   }
}
