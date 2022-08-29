using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using RestSharp;

namespace CryptoParser.Models
{
   namespace Parsers
   {

      public class BybitParser : IParser
      {
         private readonly HttpClient _client = new();
         private CVBData _cvbData;
         private readonly Dictionary<string, int> _bankToNumber = new() 
         {
            { "Tinkoff", 75 },
            { "Rosbank", 185 },
            { "Raiffaizen", 64 },
            { "QiWi", 62 },
            { "Yandex.Money", 88 },
         };

         public BybitParser()
         {
            _cvbData = Constants.GetCVBData(CVBType.Bybit);
         }

         public async Task UpdateDataAsync()
         {
            Logger.Info("Start parse Binance prices");

            await Task.WhenAll(parseOffersAsync(), parseMarketPricesAsync());

            Logger.Info("Binance prices parsed");
         }

         private async Task parseOffersAsync()
         {
            Logger.Info($"Parse Binance offers process started");

            List<Task<Offer>> tasks = new();

            foreach (string bank in _cvbData.Banks)
            {
               foreach (string currency in _cvbData.Currencies)
               {
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Buy));
                  tasks.Add(parseOfferAsync(bank, currency, TradeType.Sell));
               }
            }
            Logger.Info($"Parse Binance offers process passed, {tasks.Count} tasks created");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(task => ServicesContainer.Get<CVBsData>().AddOffer(task.Result));

            Logger.Info($"Parse Binance offers process finished");
         }

         private async Task<Offer> parseOfferAsync(string bank, string currency, TradeType tradeType)
         {

            //try
            //{
            //string data = JsonConvert.SerializeObject(
            //   new
            //   {
            //      tokenId = "USDT",
            //      currencyId = "RUB",
            //      payment = 185,
            //      side = 1,
            //      size = 10,
            //      page = 1,
            //      amount = 1000,
            //   });

            //var data = new[]
            //   {
            //      new KeyValuePair<string, string>("tokenId", "USDT"),
            //      new KeyValuePair<string, string>("currencyId", "RUB"),
            //      new KeyValuePair<string, string>("payment", "185"),
            //      new KeyValuePair<string, string>("side", "1"),
            //      new KeyValuePair<string, string>("size", "10"),
            //      new KeyValuePair<string, string>("page", "1"),
            //      new KeyValuePair<string, string>("amount", "1000"),
            //   };

            //client.PostAsync(endPoint, new FormUrlEncodedContent(data));

            //var requestUri = "https://api2.bybit.com/spot/api/otc/item/list";
            //var content = new StringContent("userId=&tokenId=USDT&currencyId=RUB&payment=185&side=1&size=10&page=1&amount=", Encoding.UTF8, "application/x-www-form-urlencoded");
            //var a = new FormUrlEncodedContent( (data);
            //var response = await _client.PostAsync(requestUri, content);
            //var response = await PostFormUrlEncoded(requestUri, data);
            //var responseString = await response.Content.ReadAsStringAsync();
            //var responseJson = JObject.Parse(responseString);

            //var minPrice = responseJson["data"][0]["adv"]["price"];

            //Logger.Info($"Binance Offer: {bank}, {currency}, {tradeType.TypeToString()} parsed successfully");

            //var url = "https://api2.bybit.com/spot/api/otc/item/list";

            //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpRequest.Method = "POST";

            //httpRequest.ContentType = "application/x-www-form-urlencoded";

            //var data = "userId=&tokenId=USDT&currencyId=RUB&payment=185&side=1&size=10&page=1&amount=";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //   streamWriter.Write(data);
            //}

            //var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //   var result = streamReader.ReadToEnd();
            //}

            //var client = new RestClient("https://api2.bybit.com/spot/api/otc/item/list");
            //var request = new RestRequest("https://api2.bybit.com/spot/api/otc/item/list", Method.Post);
            //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            ////request.AddHeader("Cookie", "_abck=7D7C9250EFD9A9B8C9DED4F5FC7DEA35~-1~YAAQjYnvUNGPRMmCAQAA7UOV1AiLyYn5X9K2wFldUR6220idfbHoVLqfwyRJQzmXCyyUJ/fbdmnYYCLc+sfUQqoLrq440GEx/C+x8Gk5iYz9Kef9HyIFtt+UjdSyg6wGIiojk/IKkVNv6YQQU6SLHaErecyV0xi1+ZohqMAW3sAzE85mSILp3NzWi8MQX6/EIOoAe/QPAHBgwWZQFW39wXqiWD/mD6qlVhW9KyVl7gT9iIuxNDQXs6zA4JXMIC5gmcQepL4W4v6/aEItpae8F1gp4iukGxnXZAhCEhs8mqiRAqT7cZ84eTaaIiXzUhjNTfMXmBg7wIZj+o+RJoNRsZCwMhRV3i2rn+kYuAElVD5r8OJxacQ9VOtoKC9gVu1JvHEiaU2d~-1~-1~-1; ak_bmsc=25D2E3D4E409233BF8E78EDE8CCBF77E~000000000000000000000000000000~YAAQjYnvUNKPRMmCAQAA7UOV1BBJDLMgsBrfayvkhh/Rb4fL7bwytlYIuB/zT3pGATWMCFJw7OinjtoVsvGUEX/MtWfOmiQSgW1yDBHZY6oiDWqcHsrFXDlTdwOD4y3H/giEMRiK/XRP0leEq/BLirMVH3nPFE0knLxrnYXN3EiWn1UEGYQu48Dm4ooAsHXHLrczxHPkam1TAEwJqi6rMBxHkDxqhazm0+C6gv0nyze9tkRR8M33NuK8iRYcJGQYseml5FCsFlE8Mbm3b0b+D2YhWLXSMrNDEzeMfBSEEt4nwSpaXqODXWDy/J3RxBgRkS5diCgY9rHF6lv6eE3YB0BSLNLCj+BUrxS+FM0M9qvStN5zeIwfz9Y6y2Ss; bm_sv=4824B74F5E2027DA903C7C5A4A595501~YAAQjYnvUDKeRMmCAQAAO7+Y1BBl6UU8iIpWPa1jR2Htd64jc7XyOEDgE36oS1iB97d5ObyY5rkDNUK7vNra6oOMAbvlyZ+tldgWalt7ITUTUBOHVDxpP4vGMRoJgw7o2zywQfvy1/XIu4FLL/BEyy+oUU1Av6ww32nMvxOABPBmVhIYsnV7Tn6gKUrvouKRjQvGm7Iwk0QNb52uKB2XZiUl54Nn7q9avHd4Cko9gFBPQuk8kH278xk8AjqkUO0i~1; bm_sz=2986FEFE225E62CC6E224E8B787D3453~YAAQjYnvUNOPRMmCAQAA7UOV1BAjjFTyBc2WxbTUyQdgsTa7wtnLAdIh9xRgDG9c4obzuep8pcODY3rC2G5opgEBCE5YWaRffr048mViVWa7x6q+uDoWVWv/N2/UupGEnFLn4nL4aQlaEc8XZQWzCAnireGWbIfTOBR1hxvKPK+O99eDWJ1pWYApkEf00BpU/g8hC2MhLz3JW6nOPXLZnWqjrCB877ZjdbVJMkvezAiG/B+bA7tZLjCb550ApEb+HJ8T/3orJZ80GKD5ROYwnzIB7lu4+Dkj7wBZWzq7Pqhg9w==~3490096~3360305");
            //request.AddParameter("tokenId", "USDT");
            //request.AddParameter("currencyId", "RUB");
            //request.AddParameter("payment", "185");
            //request.AddParameter("size", "1");
            //request.AddParameter("side", "1");
            //request.AddParameter("page", "1");
            //var response = await client.ExecuteAsync(request);
            //Console.WriteLine(response.Content);

            try
            {
               var requestUri = $"https://www.bybit.com/fiat/trade/otc/?actionType=0&token=USDT&fiat=RUB&paymentMethod=75";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);
            }
            catch (Exception ex)
            {
               var b = ex.Message;
               int a = 8;
            }
            
            return new Offer(CVBType.Bybit, bank, currency, tradeType, 0, "OK");//(float)minPrice, "OK");

            //var postParams = new List<KeyValuePair<string, object>>()
            //{
            //      new KeyValuePair<string, object>("tokenId", "USDT"),
            //      new KeyValuePair<string, object>("currencyId", "RUB"),
            //      new KeyValuePair<string, object>("payment", 185),
            //      new KeyValuePair<string, object>("side", 1),
            //      new KeyValuePair<string, object>("size", 1),
            //      new KeyValuePair<string, object>("page", 1),
            //      new KeyValuePair<string, object>("amount", 1000)
            //};

            //var formString = string.Join("&", postParams.Select(x => string.Format("{0}={1}", x.Key, x.Value)));

            //var bytes = Encoding.ASCII.GetBytes(formString);

            ////Create a POST webrequest
            //var request = WebRequest.CreateHttp("https://api2.bybit.com/spot/api/otc/item/list");
            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";

            //using (var reqStream = await request.GetRequestStreamAsync())
            //{
            //   //Write bytes to the request stream
            //   await reqStream.WriteAsync(bytes, 0, bytes.Length);
            //}

            //WebResponse response = null;
            //try
            //{
            //   response = await request.GetResponseAsync();
            //}
            //catch (WebException e)
            //{
            //   int c = 9;
            //   //Something went wrong with our request. Return the exception message.
            //   //return e.Message;
            //}

            //using (var respStream = response.GetResponseStream())
            //{
            //   //Create a streamreader to read the website's response, then return it as a string
            //   using (var reader = new StreamReader(respStream))
            //   {
            //      var a = await reader.ReadToEndAsync();
            //      int b = 7;
            //   }
            //}
            //return new Offer(CVBType.Bybit, bank, currency, tradeType, 0, "OK");//(float)minPrice, "OK");
            //}
            //catch (HttpRequestException e)
            //{
            //   Logger.Info($"Binance parse exeption: {e.Message}");
            //   return new Offer(CVBType.Bybit, bank, currency, tradeType, 0, "BadRequest");
            //}
            //catch (Exception e)
            //{
            //   Logger.Info($"Binance read answer exeption: {e.Message}");
            //   return new Offer(CVBType.Bybit, bank, currency, tradeType, 0, e.Message);
            //}
         }



         //public static async Task<HttpResponseMessage> PostFormUrlEncoded(string url, IEnumerable<KeyValuePair<string, string>> postData)
         //{
         //   using (var httpClient = new HttpClient())
         //   {
         //      using (var content = new FormUrlEncodedContent(postData))
         //      {
         //         content.Headers.Clear();
         //         content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

         //         return await httpClient.PostAsync(url, content);
         //      }
         //   }
         //}

         private async Task parseMarketPricesAsync()
         {
            Logger.Info($"Parse Binance marketPrices process started");

            List<Task<MarketRate>> tasks = new();
            _cvbData.Currencies.ToList().ForEach(currency => tasks.Add(parseMarketPriceAsync(currency)));

            Logger.Info($"Parse Binance marketPrices process passed, {tasks.Count} tasks created");

            await Task.WhenAll(tasks.ToArray());
            tasks.ForEach(rate => ServicesContainer.Get<CVBsData>().AddMarketPrice(rate.Result));

            Logger.Info($"Parse Binance marketPrices process finished");
         }

         private async Task<MarketRate> parseMarketPriceAsync(string currency)
         {
            if (currency == "USDT")
               return new MarketRate(CVBType.Bybit, currency, 1, "OK");

            try
            {
               var requestUri = "https://api-testnet.bybit.com/v2/public/tickers";
               var response = await _client.GetAsync(requestUri);
               var responseString = await response.Content.ReadAsStringAsync();
               var responseJson = JObject.Parse(responseString);

               float rate = -1;
               foreach (var item in responseJson["result"])
               {
                  if (item["symbol"].ToString() == $"{currency}USDT")
                     rate = (float)item["bid_price"];
               }

               if (rate != -1)
               {
                  Logger.Info($"Binance MarketPrice: {currency} parsed successfully");
                  return new MarketRate(CVBType.Bybit, currency, rate, "OK");
               }
               else
               {
                  Logger.Info($"{currency} Binance MarketPrice not collected");
                  return new MarketRate(CVBType.Bybit, currency, rate, "Not Collected");
               }
            }
            catch (HttpRequestException e)
            {
               Logger.Info($"Binance parse exeption: {e.Message}");
               return new MarketRate(CVBType.Bybit, currency, 0, "BadRequest");
            }
            catch (Exception e)
            {
               Logger.Info($"Binance read answer exeption: {e.Message}");
               return new MarketRate(CVBType.Bybit, currency, 0, e.Message);
            }
         }
      }
   }
}
