using System;

namespace OSPSuite.Serializer.Attributes
{
   public class AttributeMapperNotFoundException : Exception
   {
      private static string errorMessage(Type type) => $"Unable to find an attribute mapper for '{type}'";

      public AttributeMapperNotFoundException(Type typeOfObject)
         : base(errorMessage(typeOfObject))
      {
      }
   }
}