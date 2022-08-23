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
}