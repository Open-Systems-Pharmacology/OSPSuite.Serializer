using System;
using System.Collections;
using System.Text;

namespace OSPSuite.Serializer
{
   public class AmbiguousSerializerException : Exception
   {
      public override string Message { get; } = string.Empty;

      public AmbiguousSerializerException(IEnumerable allPossibleImplementations, Type typeOfObjectToSerialize)
      {
         var sb = new StringBuilder();
         sb.AppendLine($"Ambiguous serializer implementation for '{typeOfObjectToSerialize}'.");
         sb.AppendLine("Possible implementations are:");
         foreach (object possibleImplementation in allPossibleImplementations)
         {
            sb.AppendLine($"\tSerializer<{possibleImplementation.GetType().FullName}>");
         }

         Message = sb.ToString();
      }

   }
}