namespace CryptoParser.Models
{
   public class СryptoСurrency
   {
      public string Name { get; private set; }
      public float BuyPrice { get; private set; }
      public float SellPrice { get; private set; }

      public СryptoСurrency(string name, float buyPrice, float sellPrice)
      {
         Name = name;
         BuyPrice = buyPrice;
         SellPrice = sellPrice;
      }
   }
}