using System.Collections.Generic;

namespace OSPSuite.Serializer
{
   public static class MemberNamingConventions
   {
      public static IReadOnlyList<MemberNamingConvention> AllConventions = new[]
      {
         new MemberNamingConvention("_"),
         new MemberNamingConvention("m_"),
         new MemberNamingConvention("<", ">k__BackingField"),
         new MemberNamingConvention(),
      };
   }

   public class MemberNamingConvention
   {
      private readonly string _prefix;
      private readonly string _suffix;

      public MemberNamingConvention(string prefix = "", string suffix = "")
      {
         _prefix = prefix;
         _suffix = suffix;
      }

      public string Transform(string name) => $"{_prefix}{name}{_suffix}";

      public bool Matches(string memberName, string propertyName) => string.Equals(memberName.ToLowerInvariant(), Transform(propertyName).ToLowerInvariant());

      public bool Transformed(string name) =>
         name.StartsWith(_prefix) &&
         name.EndsWith(_suffix);

      public string Original(string input)
      {
         var original = input.Substring(_prefix.Length);
         return original.Substring(0, original.Length - _suffix.Length);
      }
   }
}