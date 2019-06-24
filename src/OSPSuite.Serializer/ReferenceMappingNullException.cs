using System;

namespace OSPSuite.Serializer
{
   public class ReferenceMappingNullException : Exception
   {
      private static string exceptionMessage(Type type, string expression) =>
         $"Reference attribute mapper was not defined in the attribute repository but is used for:\ntype = '{type}'\nexpression = '{expression}'.\n";

      public ReferenceMappingNullException(Type typeToMap, string expression)
         : base(exceptionMessage(typeToMap, expression))
      {
      }
   }
}