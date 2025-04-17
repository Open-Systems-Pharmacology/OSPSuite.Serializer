namespace OSPSuite.Serializer.Extensions
{
   public static class StringExtensions
   {
      public static string ToPascalCase(this string stringToConvert)
      {
         if (string.IsNullOrEmpty(stringToConvert))
            return stringToConvert;

         var allChars = stringToConvert.ToCharArray();

         var firstChar = allChars[0];

         if (!char.IsUpper(firstChar))
            return stringToConvert;

         allChars[0] = char.ToLower(firstChar);
         return new string(allChars);
      }
   }
}