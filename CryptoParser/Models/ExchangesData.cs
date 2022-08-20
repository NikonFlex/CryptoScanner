namespace CryptoParser.Models
{
   public class ExchangesData
   {
      private List<Offer> _offers = new();

      public void ClearOffers() => _offers.Clear();
      public void AddOffer(Offer offer) => _offers.Add(offer);
      
      public Offer GetOffer(string exchange, string bank, string currency, TradeType tradeType)
      {
         return _offers.FirstOrDefault(offer => offer.Exchange == exchange &&
                                offer.Bank == bank &&
                                offer.Currency == currency &&
                                offer.TradeType == tradeType);
      }
   }
}
