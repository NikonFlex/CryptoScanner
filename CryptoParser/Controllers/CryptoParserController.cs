using Microsoft.AspNetCore.Mvc;

namespace CryptoParser.Controllers
{
   [ApiController]
   [Route("")]
   public class DefaultController : Controller
   {
      [HttpGet]
      public float Get()
      {
         return Services.ServicesContainer.Get<Services.ParserManager>().GetNumber();
      }

   }

   [ApiController]
   [Route("[controller]")]
   public class CryptoParser : ControllerBase
   {
      [HttpGet("GetAllUsers")]
      public int ParsePrice()
      {
         return 1;
      }
   }
}