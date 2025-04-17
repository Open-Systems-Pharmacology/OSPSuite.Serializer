using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_NullableBoolAttributeMapper : ContextSpecification<NullableBoolAttributeMapper<TestSerializationContext>>
   {
      protected TestSerializationContext _context;

      protected override void Context()
      {
         sut = new NullableBoolAttributeMapper<TestSerializationContext>();
         _context = new TestSerializationContext();
      }
   }

   public class When_converting_a_nullable_bool_value_to_string : concern_for_NullableBoolAttributeMapper
   {
      private bool? _nullBool;
      private bool? _nullBoolNotEmpty;

      protected override void Context()
      {
         base.Context();
         _nullBool = null;
         _nullBoolNotEmpty = true;
      }

      [Observation]
      public void should_return_the_null_string_for_a_null_value()
      {
         sut.Convert(_nullBool, _context).ShouldBeNull();
      }

      [Observation]
      public void should_return_a_valid_string_for_a_value_that_is_not_null()
      {
         sut.Convert(_nullBoolNotEmpty, _context).ShouldBeEqualTo("1");
      }
   }

   public class When_converting_a_string_to_a_nullable_bol_value : concern_for_NullableBoolAttributeMapper
   {
      [Observation]
      public void should_return_null_for_an_empty_string()
      {
         sut.ConvertFrom(string.Empty, _context).ShouldBeEqualTo(null);
      }

      [Observation]
      public void should_return_a_nullable_bool_with_the_accurate_value_for_a_non_empty_string()
      {
         var value = sut.ConvertFrom("1", _context).ConvertedTo<bool?>();
         value.ShouldNotBeNull();
         value.Value.ShouldBeEqualTo(true);
      }
   }
}