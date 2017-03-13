using System;

namespace OSPSuite.Serializer.Reflection
{
   public interface IMemberAccessor
   {
      string Name { get; }
      Type MemberType { get; }
      object GetValue(object source);
      void SetValue(object destination, object value);
   }
}