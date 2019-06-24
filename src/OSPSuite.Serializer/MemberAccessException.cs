using System;
using System.Reflection;

namespace OSPSuite.Serializer
{
   public class MemberAccessException : Exception
   {
      private static string errorMessageMemberInfo(MemberInfo memberInfo) => $"Cannot create member access for '{memberInfo}'";

      private static string errorMessageMemberName(Type objectType, Type propertyType, string memberName) =>
         $"Could not find property '{memberName}' with type '{propertyType}' in '{objectType}'";

      public MemberAccessException(MemberInfo memberInfo) : base(errorMessageMemberInfo(memberInfo))
      {
      }

      public MemberAccessException(Type objectType, Type propertyType, string memberName)
         : base(errorMessageMemberName(objectType, propertyType, memberName))
      {
      }
   }
}