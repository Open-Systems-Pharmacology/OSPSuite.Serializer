using System;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Attributes
{
   public class AttributeMappingException : Exception
   {
      private const string _exceptionMessage =
         "Unsupported attribute mapping found for:\ntype = '{0}'\nexpression = '{1}'.\n";

      public AttributeMappingException(Type typeToMap, string expression)
         : base(_exceptionMessage.FormatWith(typeToMap.ToString(), expression))
      {
      }
   }
}