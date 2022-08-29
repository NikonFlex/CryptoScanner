namespace CryptoParser.Models
{
   namespace Tables
   {
      public interface ITable
      {
         public List<List<object>> CreateTable(int balance, SpreadType spreadType);
      }
   }
}
