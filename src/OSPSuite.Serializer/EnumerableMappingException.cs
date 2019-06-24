using System;

namespace OSPSuite.Serializer
{
   public class EnumerableMappingException : Exception
   {
      private static string exceptionMessage(Type objectType, string mappingName) =>
         $"No add function defined for the enumerable mapping for:\ntype = '{objectType}'\nproperty = '{mappingName}'.\nPlease use MapEnumerable(x=>x.{mappingName}).WithAdd(x=>x..)";

      public EnumerableMappingException(IEnumerableMap enumerableMap) :
         base(exceptionMessage(enumerableMap.ObjectType, enumerableMap.MappingName))
      {
      }
   }
}