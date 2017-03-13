using System;
using System.Reflection;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer
{
   public class MemberAccessException : Exception
   {
      private const string _errorMessageMemberInfo = "Cannot create member access for '{0}'";
      private const string _errorMessageMemberName = "Could not find property '{2}' with type '{1}' in '{0}'";

      public MemberAccessException(MemberInfo memberInfo) : base(_errorMessageMemberInfo.FormatWith(memberInfo))
      {
      }

      public MemberAccessException(Type objectType, Type propertyType, string memberName)
         : base(_errorMessageMemberName.FormatWith(objectType, propertyType, memberName))
      {
      }
   }
}