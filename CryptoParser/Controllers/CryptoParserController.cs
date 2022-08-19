using Microsoft.AspNetCore.Mvc;

namespace CryptoParser.Controllers
{
   [ApiController]
   [Route("")]
   public class DefaultController : Controller
   {
      [HttpGet]
      public string Get()
      {
         return DateTime.UtcNow.ToString();
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