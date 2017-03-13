using System;
using System.Collections;
using System.Text;

namespace OSPSuite.Serializer
{
   public class AmbigousSerializerException : Exception
   {
      private readonly string _message = string.Empty;

      public AmbigousSerializerException(IEnumerable allPossibleImplementations, Type typeOfObjectToSerialize)
      {
         var sb = new StringBuilder();
         sb.AppendLine(string.Format("Ambiguous serializer implementation for '{0}'.", typeOfObjectToSerialize));
         sb.AppendLine("Possible implementations are:");
         foreach (object possibleImplementation in allPossibleImplementations)
         {
            sb.AppendLine(string.Format("\tSerializer<{0}>", possibleImplementation.GetType().FullName));
         }

         _message = sb.ToString();
      }

      public override string Message
      {
         get { return _message; }
      }
   }
}