using Newtonsoft.Json;

namespace CryptoParser
{
   namespace Models
   {
      public class CVBsData
      {
         [JsonProperty]
         public DateTime ParseTime { get; private set; }
         [JsonProperty]
         private List<Offer> _offers = new();
         [JsonProperty]
         private List<MarketRate> _marketRates = new();

         public void UpdateParseTime() => ParseTime = DateTime.UtcNow;

         public bool IsEmpty() => _offers.Count == 0 && _marketRates.Count == 0;

         public void ClearData()
         {
            _offers.Clear();
            _marketRates.Clear();
         }

         public void AddOffer(Offer offer)
         {
            Logger.Info($"Add offer: bank {offer.Bank} currency {offer.Currency}");
            _offers.Add(offer);
         }

         public void AddMarketPrice(MarketRate marketPrice) => _marketRates.Add(marketPrice);

         public Offer? GetOffer(CVBType cvb, Bank bank, Currency currency, TradeType tradeType) => _offers.FirstOrDefault(offer => offer.CVB == cvb &&
                                                                                                                                   offer.Bank == bank &&
                                                                                                                                   offer.Currency == currency &&
                                                                                                                                   offer.TradeType == tradeType);

         public MarketRate? GetMarketRate(CVBType cvb, Currency currency) => _marketRates.FirstOrDefault(price => price.CVB == cvb &&
                                                                                                                 price.Currency == currency);

      }
   }
}
