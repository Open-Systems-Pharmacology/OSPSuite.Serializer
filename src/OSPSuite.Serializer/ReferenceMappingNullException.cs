using System;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer
{
   public class ReferenceMappingNullException : Exception
   {
      private const string _exceptionMessage = "Reference attribute mapper was not defined in the attribute repository but is used for:\ntype = '{0}'\nexpression = '{1}'.\n";

      public ReferenceMappingNullException(Type typeToMap, string expression)
         : base(_exceptionMessage.FormatWith(typeToMap.ToString(), expression))
      {
      }
   }
}