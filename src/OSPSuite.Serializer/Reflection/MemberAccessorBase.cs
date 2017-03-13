using System;

namespace OSPSuite.Serializer.Reflection
{
   internal abstract class MemberAccessorBase : IMemberAccessor
   {
      protected readonly Type _memberType;
      protected readonly string _name;

      protected MemberAccessorBase(string name, Type memberType)
      {
         _name = name;
         _memberType = memberType;
      }

      public string Name
      {
         get { return _name; }
      }

      public Type MemberType
      {
         get { return _memberType; }
      }

      public abstract object GetValue(object source);
      public abstract void SetValue(object destination, object value);
   }
}