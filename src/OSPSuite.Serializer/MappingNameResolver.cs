namespace OSPSuite.Serializer
{
   public interface IMappingNameResolver
   {
      string MappingNameFor(string input);
   }

   internal class MappingNameResolver : IMappingNameResolver
   {
      public string MappingNameFor(string input)
      {
         if (string.IsNullOrEmpty(input))
            return input;

         //remove m_ or _ at the beginning
         string trimedInput = input;
         foreach (var convention in MemberNamingConventions.AllConventions)
         {
            if (convention.Transformed(trimedInput))
            {
               trimedInput = convention.Original(input);
               break;
            }
         }

         if (string.IsNullOrEmpty(trimedInput))
            return input;

         return trimedInput;
      }
   }
}