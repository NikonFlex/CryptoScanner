namespace CryptoParser.Models
{
   namespace Tables
   {
      public class HardTableAttribute : Attribute
      {
         public readonly string Name;

         public HardTableAttribute(string name)
         {
            Name = name;
         }
      }
   }
}
