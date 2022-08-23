namespace CryptoParser.Models
{
   namespace Tables
   {
      public class TableAttribute : Attribute
      {
         public readonly string Name;

         public TableAttribute(string name)
         {
            Name = name;
         }
      }
   }
}
