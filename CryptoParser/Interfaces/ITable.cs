namespace CryptoParser
{
   namespace Tables
   {
      public interface ITable
      {
         public List<List<object>> CreateTable(int balance, Models.SpreadType spreadType);
      }
   }
}
