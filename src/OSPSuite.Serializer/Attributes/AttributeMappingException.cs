using System;

namespace OSPSuite.Serializer.Attributes
{
   public class AttributeMappingException : Exception
   {
      public AttributeMappingException(Type typeToMap, string expression)
         : base($"Unsupported attribute mapping found for:\ntype = '{typeToMap}'\nexpression = '{expression}'.\n")
      {
      }
   }
}