using CryptoParser.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptoParser
{
   namespace Controllers
   {
      [ApiController]
      [Route("")]
      public class DefaultController : Controller
      {
         //[HttpGet("GetMarketRate")]
         //public float? GetMarketRate(string cvbDTO, string currencyDTO)
         //{
         //   var cvb = Utils.GetCVBTypeFrom(cvbDTO);
         //   var currency = Utils.GetCurrencyTypeFrom(currencyDTO);
         //   return ServicesContainer.Get<Models.CVBsData>().GetMarketRate(cvb, currency)?.Price;
         //}

         //[HttpGet("GetOffer")]
         //public float? GetOffer(string cvbDTO, string bankDTO, string currencyDTO, string tradeTypeDTO)
         //{
         //   var cvb = Utils.GetCVBTypeFrom(cvbDTO);
         //   var bank = Utils.GetBankTypeFrom(bankDTO);
         //   var currency = Utils.GetCurrencyTypeFrom(currencyDTO);
         //   var tradeType = Utils.GetTradeTypeFrom(tradeTypeDTO);
         //   return ServicesContainer.Get<Models.CVBsData>().GetOffer(cvb, bank, currency, tradeType)?.Price;
         //}

         [HttpGet("UpdateTableWithID")]
         public void UpdateTableWithID(string spreadSheetId, string spreadType, int balance)
         {
            ServicesContainer.Get<CVBsDataManager>().SetData(getActaulData());
            Logger.Info("data set");
            var filler = new GoogleSheetRequestsCreator(new GoogleTable() { ID = spreadSheetId, Spread = spreadType, Balance = balance });
            filler.UpdateSpreadSheet();
            Logger.Info($"spreadsheet {spreadSheetId} updated");
         }

         private CVBsData getActaulData()
         {
            CVBsData dataFromJson = Utils.ReadCVBsDataFromFile();
            if (dataFromJson.IsEmpty() == false)
            {
               if ((DateTime.UtcNow - dataFromJson.ParseTime).TotalSeconds > 30)
               {
                  Logger.Info("data must update");
                  return ServicesContainer.Get<Parsing.ParsersManager>().ParseData();
               }
               else
               {
                  Logger.Info("json data is vaild");
                  return dataFromJson;
               }
            }
            else
            {
               Logger.Info("json was empty, data updated");
               return ServicesContainer.Get<Parsing.ParsersManager>().ParseData();
            }
         }

         [HttpGet]
         public string Get() => DateTime.UtcNow.ToString();
      }
   }
}