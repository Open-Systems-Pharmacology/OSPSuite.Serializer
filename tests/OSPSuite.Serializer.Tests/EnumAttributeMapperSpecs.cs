using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_enum_attribute_mapper_specs<T> : ContextSpecification<EnumAttributeMapper<T, TestSerializationContext>>
   {
      protected TestSerializationContext _context;

      protected override void Context()
      {
         _context = new TestSerializationContext();
      }
   }

   public class When_mapping_an_enumeration_using_the_enum_attribute_mapper : concern_for_enum_attribute_mapper_specs<CompoundType>
   {
      protected override void Context()
      {
         sut = new EnumAttributeMapper<CompoundType, TestSerializationContext>();
      }

      [Observation]
      public void should_be_able_to_map_the_enumeration_value_from_the_string()
      {
         sut.ConvertFrom(CompoundType.Acid.ToString(), _context).ShouldBeEqualTo(CompoundType.Acid);
         sut.ConvertFrom(CompoundType.Base.ToString(), _context).ShouldBeEqualTo(CompoundType.Base);
         sut.ConvertFrom(CompoundType.Neutral.ToString(), _context).ShouldBeEqualTo(CompoundType.Neutral);
      }

      [Observation]
      public void should_be_able_to_map_the_enumeration_value()
      {
         sut.Convert(CompoundType.Acid, _context).ShouldBeEqualTo(CompoundType.Acid.ToString());
         sut.Convert(CompoundType.Base, _context).ShouldBeEqualTo(CompoundType.Base.ToString());
         sut.Convert(CompoundType.Neutral, _context).ShouldBeEqualTo(CompoundType.Neutral.ToString());
      }
   }

   public class When_mapping_a_class_that_is_not_an_enumeration_using_the_enum_attribute_mapper : concern_for_enum_attribute_mapper_specs<Project>
   {
      [Observation]
      public void should_throw_exception()
      {
         The.Action(() => sut = new EnumAttributeMapper<Project, TestSerializationContext>()).ShouldThrowAn<ArgumentException>();
      }
   }
}