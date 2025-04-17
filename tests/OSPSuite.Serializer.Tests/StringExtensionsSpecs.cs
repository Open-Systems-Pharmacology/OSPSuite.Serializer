using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Extensions;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_string_extensions : StaticContextSpecification
   {
      protected override void Context()
      {
      }
   }

   public class When_converting_a_string_in_pascal_case : concern_for_string_extensions
   {
      [Observation]
      public void should_return_the_same_string()
      {
         "pascalCaseString".ToPascalCase().ShouldBeEqualTo("pascalCaseString");
      }
   }

   public class When_converting_a_null_or_empty_string_to_pascal_case : concern_for_string_extensions
   {
      [Observation]
      public void should_return_a_string_null_or_empty()
      {
         string.IsNullOrEmpty("".ToPascalCase()).ShouldBeTrue();
      }
   }

   public class When_converting_a_string_that_is_not_in_pascal_case : concern_for_string_extensions
   {
      [Observation]
      public void should_return_a_string_in_pascal_case()
      {
         "OneCamelCase".ToPascalCase().ShouldBeEqualTo("oneCamelCase");
      }
   }
}