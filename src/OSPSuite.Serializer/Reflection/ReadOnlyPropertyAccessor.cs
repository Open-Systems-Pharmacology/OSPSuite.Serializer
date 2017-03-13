using System.Reflection;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Serializer.Reflection
{
   internal class ReadOnlyPropertyAccessor : MemberAccessorBase
   {
      protected readonly GetHandler _getHandler;
      private readonly PropertyInfo _readProperty;

      public ReadOnlyPropertyAccessor(PropertyInfo readProperty) : base(readProperty.Name, readProperty.PropertyType)
      {
         _readProperty = readProperty; 
         _getHandler = DelegateFactory.CreateGet(readProperty);
      }

      public override object GetValue(object source)
      {
         return _getHandler(source);
      }

      public override void SetValue(object destination, object value)
      {
         throw new MemberAccessException(_readProperty);
      }
   }
}