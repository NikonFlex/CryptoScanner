namespace CryptoParser
{
   namespace Models
   {
      public class CVBsDataManager
      {
         private CVBsData _data;

         public CVBsData GetData() => _data;
         public void SetData(CVBsData newData) => _data = newData;
      }
   }
}
