using System.Reflection;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Serializer.Reflection
{
   internal class ReadWritePropertyAccessor : ReadOnlyPropertyAccessor
   {
      private readonly SetHandler _setHandler;

      public ReadWritePropertyAccessor(PropertyInfo propertyInfo) : base(propertyInfo) => _setHandler = DelegateFactory.CreateSet(propertyInfo);

      public override void SetValue(object destination, object value) => _setHandler(destination, value);
   }
}