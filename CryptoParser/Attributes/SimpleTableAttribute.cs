namespace CryptoParser
{
   namespace Tables
   {
      public class SimpleTableAttribute : Attribute
      {
         public readonly string Name;

         public SimpleTableAttribute(string name)
         {
            Name = name;
         }
      }
   }
}
