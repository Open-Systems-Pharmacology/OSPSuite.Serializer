using System.Reflection;

namespace OSPSuite.Serializer.Reflection
{
   internal class ReadOnlyPropertyWithPrivateFieldAccessor : ReadOnlyPropertyAccessor
   {
      private readonly FieldInfo _writeFieldInfo;

      public ReadOnlyPropertyWithPrivateFieldAccessor(PropertyInfo readProperty, FieldInfo writeFieldInfo) : base(readProperty) => 
         _writeFieldInfo = writeFieldInfo;

      public override void SetValue(object destination, object value) => 
         _writeFieldInfo.SetValue(destination, value);
   }
}