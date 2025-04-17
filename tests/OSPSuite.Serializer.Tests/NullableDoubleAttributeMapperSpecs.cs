using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_NullableDoubleAttributeMapper : ContextSpecification<IAttributeMapper>
   {
      protected TestSerializationContext _context;

      protected override void Context()
      {
         sut = new NullableDoubleAttributeMapper<TestSerializationContext>();
         _context = new TestSerializationContext();
      }
   }

   public class When_converting_a_nullable_value_to_string : concern_for_NullableDoubleAttributeMapper
   {
      private double? _nullDouble;
      private double? _nullDoubleNotEmpty;

      protected override void Context()
      {
         base.Context();
         _nullDouble = null;
         _nullDoubleNotEmpty = 5.6;
      }

      [Observation]
      public void should_return_the_null_string_for_a_null_value()
      {
         sut.Convert(_nullDouble, _context).ShouldBeNull();
      }

      [Observation]
      public void should_return_a_valid_string_for_a_value_that_is_not_null()
      {
         sut.Convert(_nullDoubleNotEmpty, _context).ShouldBeEqualTo("5.6");
      }
   }

   public class When_converting_a_string_to_a_nullable_value : concern_for_NullableDoubleAttributeMapper
   {
      [Observation]
      public void should_return_null_for_an_empty_string()
      {
         sut.ConvertFrom(string.Empty, _context).ShouldBeEqualTo(null);
      }

      [Observation]
      public void should_return_a_nullable_double_with_the_accurate_value_for_a_non_empty_string()
      {
         var value = sut.ConvertFrom("5.6", _context).ConvertedTo<double?>();
         value.ShouldNotBeNull();
         value.Value.ShouldBeEqualTo(5.6);
      }
   }
}