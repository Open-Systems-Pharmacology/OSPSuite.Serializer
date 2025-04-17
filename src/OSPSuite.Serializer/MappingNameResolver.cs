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
         var trimmedInput = input;
         foreach (var convention in MemberNamingConventions.AllConventions)
         {
            if (convention.Transformed(trimmedInput))
            {
               trimmedInput = convention.Original(input);
               break;
            }
         }

         if (string.IsNullOrEmpty(trimmedInput))
            return input;

         return trimmedInput;
      }
   }
}