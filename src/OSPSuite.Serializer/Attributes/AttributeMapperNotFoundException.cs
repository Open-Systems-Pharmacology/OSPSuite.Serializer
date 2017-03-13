using System;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Attributes
{
   public class AttributeMapperNotFoundException : Exception
   {
      private const string _errorMessage = "Unable to find an attribute mapper for '{0}'";

      public AttributeMapperNotFoundException(Type typeOfObject)
         : base(_errorMessage.FormatWith(typeOfObject))
      {
      }
   }
}