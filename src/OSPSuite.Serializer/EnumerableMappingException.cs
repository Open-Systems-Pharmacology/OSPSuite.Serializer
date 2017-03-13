using System;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer
{
   public class EnumerableMappingException : Exception
   {
      private const string _exceptionMessage =
         "No add function defined for the enumerable mapping for:\ntype = '{0}'\nproperty = '{1}'.\n" +
         "Please use MapEnumerable(x=>x.{1}).WithAdd(x=>x..)";

      public EnumerableMappingException(IEnumerableMap enumerableMap) :
         base(_exceptionMessage.FormatWith(enumerableMap.ObjectType, enumerableMap.MappingName))
      {
      }
   }
}