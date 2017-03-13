using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_bool_attribute_mapper : ContextSpecification<IAttributeMapper>
   {
      protected TestSerializationContext _context;

      protected override void Context()
      {
         sut = new BoolAttributeMapper<TestSerializationContext>();
         _context = new TestSerializationContext();
      }
   }

   public class When_converting_a_boolean_value_to_string : concern_for_bool_attribute_mapper
   {
      [Observation]
      public void should_return_1_for_a_true_value()
      {
         sut.Convert(true, _context).ShouldBeEqualTo("1");
      }

      [Observation]
      public void should_return_0_for_a_false_value()
      {
         sut.Convert(false, _context).ShouldBeEqualTo("0");
      }

      [Observation]
      public void should_throw_an_exception_for_any_other_value()
      {
         The.Action(() => sut.Convert("tralala", _context)).ShouldThrowAn<FormatException>();
      }
   }

   public class When_converting_a_string_to_a_boolean_value : concern_for_bool_attribute_mapper
   {
      [Observation]
      public void should_return_true_for_value_equals_to_1()
      {
         sut.ConvertFrom("1", _context).ShouldBeEqualTo(true);
      }

      [Observation]
      public void should_return_false_for_value_equals_to_0()
      {
         sut.ConvertFrom("0", _context).ShouldBeEqualTo(false);
      }

      [Observation]
      public void should_throw_an_exception_for_any_other_value()
      {
         The.Action(() => sut.ConvertFrom("tralala", _context)).ShouldThrowAn<FormatException>();
      }
   }
}