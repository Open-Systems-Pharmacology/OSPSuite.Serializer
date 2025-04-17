using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_NullableFloatAttributeMapper : ContextSpecification<NullableFloatAttributeMapper<TestSerializationContext>>
   {
      protected TestSerializationContext _context;

      protected override void Context()
      {
         sut = new NullableFloatAttributeMapper<TestSerializationContext>();
         _context = new TestSerializationContext();
      }
   }

   public class When_converting_a_nullable_float_value_to_string : concern_for_NullableFloatAttributeMapper
   {
      private float? _nullFloat;
      private float? _nullFloatNotEmpty;

      protected override void Context()
      {
         base.Context();
         _nullFloat = null;
         _nullFloatNotEmpty = 5.6F;
      }

      [Observation]
      public void should_return_the_null_string_for_a_null_value()
      {
         sut.Convert(_nullFloat, _context).ShouldBeNull();
      }

      [Observation]
      public void should_return_a_valid_string_for_a_value_that_is_not_null()
      {
         sut.Convert(_nullFloatNotEmpty, _context).ShouldBeEqualTo("5.6");
      }
   }

   public class When_converting_a_string_to_a_nullable_float_value : concern_for_NullableFloatAttributeMapper
   {
      [Observation]
      public void should_return_null_for_an_empty_string()
      {
         sut.ConvertFrom(string.Empty, _context).ShouldBeEqualTo(null);
      }

      [Observation]
      public void should_return_a_nullable_double_with_the_accurate_value_for_a_non_empty_string()
      {
         var value = sut.ConvertFrom("5.6", _context).ConvertedTo<float?>();
         value.ShouldNotBeNull();
         value.Value.ShouldBeEqualTo(5.6F);
      }
   }
}