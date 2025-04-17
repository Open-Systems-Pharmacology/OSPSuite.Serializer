using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_NullableIntAttributeMapper : ContextSpecification<NullableIntAttributeMapper<TestSerializationContext>>
   {
      protected TestSerializationContext _context;

      protected override void Context()
      {
         sut = new NullableIntAttributeMapper<TestSerializationContext>();
         _context = new TestSerializationContext();
      }
   }

   public class When_converting_a_nullable_int_value_to_string : concern_for_NullableIntAttributeMapper
   {
      private int? _nullInt;
      private int? _nullIntNotEmpty;

      protected override void Context()
      {
         base.Context();
         _nullInt = null;
         _nullIntNotEmpty = 5;
      }

      [Observation]
      public void should_return_the_null_string_for_a_null_value()
      {
         sut.Convert(_nullInt, _context).ShouldBeNull();
      }

      [Observation]
      public void should_return_a_valid_string_for_a_value_that_is_not_null()
      {
         sut.Convert(_nullIntNotEmpty, _context).ShouldBeEqualTo("5");
      }
   }

   public class When_converting_a_string_to_a_nullable_int_value : concern_for_NullableIntAttributeMapper
   {
      [Observation]
      public void should_return_null_for_an_empty_string()
      {
         sut.ConvertFrom(string.Empty, _context).ShouldBeEqualTo(null);
      }

      [Observation]
      public void should_return_a_nullable_double_with_the_accurate_value_for_a_non_empty_string()
      {
         var value = sut.ConvertFrom("5", _context).ConvertedTo<int?>();
         value.ShouldNotBeNull();
         value.Value.ShouldBeEqualTo(5);
      }
   }
}