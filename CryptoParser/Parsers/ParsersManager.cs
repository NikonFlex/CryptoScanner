using CryptoParser.Models;
using System.Reflection;

namespace CryptoParser
{
   namespace Parsing
   {
      public class ParsersManager
      {
         private List<IParser> _parsers = new();

         public ParsersManager()
         {
            _parsers.AddRange(from type in Assembly.GetExecutingAssembly().GetTypes()
                              where type.IsDefined(typeof(ParserAttribute), true)
                              let constructor = type.GetConstructor(Type.EmptyTypes)
                              let instance = constructor?.Invoke(Type.EmptyTypes)
                              where instance != null
                              select (IParser)instance);
         }

         public CVBsData ParseData()
         {
            Logger.Info("Start parse cvbs");

            var marketRatesTasks = parseMarketRates();
            var offersTasks = parseOffers();

            Logger.Info("cvbs parsed");

            CVBsData data = new();

            foreach (var marketRateList in marketRatesTasks)
               marketRateList.Result.ForEach(rate => data.AddMarketPrice(rate));

            foreach (var offersList in offersTasks)
               offersList.Result.ForEach(offer => data.AddOffer(offer));

            data.UpdateParseTime();
            Utils.SaveCVBsDataToFile(data);

            return data;
         }

         private List<Task<List<Offer>>> parseOffers()
         {
            List<Task<List<Offer>>> offersTasks = new();
            _parsers.ForEach(parser => offersTasks.Add(parser.ParseOffersAsync()));
            Task.WaitAll(offersTasks.ToArray());

            Logger.Info("offers parsed");

            return offersTasks;
         }

         private List<Task<List<MarketRate>>> parseMarketRates()
         {
            List<Task<List<MarketRate>>> marketRatesTasks = new();
            _parsers.ForEach(parser => marketRatesTasks.Add(parser.ParseMarketRatesAsync()));
            Task.WaitAll(marketRatesTasks.ToArray());

            Logger.Info("market rates parsed");

            return marketRatesTasks;
         }
      }
   }
}
