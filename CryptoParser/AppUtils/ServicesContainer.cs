using Unity;

namespace CryptoParser
{
   public static class ServicesContainer
   {
      private static IUnityContainer _container = new UnityContainer();

      public static void Register<T>(T instance)
      {
         _container.RegisterInstance(instance);
      }

      public static void Register(Type t, object instance)
      {
         _container.RegisterInstance(t, instance);
      }

      public static T Get<T>()
      {
         try
         {
            return _container.Resolve<T>();
         }
         catch
         {
            return default;
         }
      }
   }
}