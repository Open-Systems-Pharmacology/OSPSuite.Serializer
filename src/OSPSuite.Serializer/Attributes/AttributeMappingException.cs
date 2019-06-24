using System;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Attributes
{
   public class AttributeMappingException : Exception
   {
      private const string _exceptionMessage =
         "Unsupported attribute mapping found for:\ntype = '{0}'\nexpression = '{1}'.\n";

      public AttributeMappingException(Type typeToMap, string expression)
         : base($"Unsupported attribute mapping found for:\ntype = '{typeToMap}'\nexpression = '{expression}'.\n")
      {
      }
   }
}