using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_xml_attribute_mapper_repository : ContextSpecification<IAttributeMapperRepository<TestSerializationContext>>
   {
      protected override void Context()
      {
         sut = new AttributeMapperRepository<TestSerializationContext>();
      }
   }

   
   public class When_adding_an_attribute_for_a_type : concern_for_xml_attribute_mapper_repository
   {
      protected override void Because()
      {
         sut.AddAttributeMapper(new IntAttributeMapper<TestSerializationContext>());
      }

      [Observation]
      public void should_be_able_to_retrieve_the_attribute_for_the_same_type()
      {
         sut.AttributeMapperFor(typeof(int)).ShouldBeAnInstanceOf < IntAttributeMapper<TestSerializationContext>>();
         sut.AttributeMapperFor<int>().ShouldBeAnInstanceOf < IntAttributeMapper<TestSerializationContext>>();
      }
   }

   
   public class When_adding_an_attribute_for_a_type_that_was_already_registered : concern_for_xml_attribute_mapper_repository
   {
      protected override void Context()
      {
         base.Context();
         sut.AddAttributeMapper(new IntAttributeMapper<TestSerializationContext>());
      }

      [Observation]
      public void should_throw_an_execption()
      {
         The.Action(() => sut.AddAttributeMapper(new IntAttributeMapper<TestSerializationContext>())).ShouldThrowAn<Exception>();
      }
   }

   
   public class When_removing_an_attribute_from_the_repository : concern_for_xml_attribute_mapper_repository
   {
      protected override void Context()
      {
         base.Context();
         sut.AddAttributeMapper(new IntAttributeMapper<TestSerializationContext>());
      }

      protected override void Because()
      {
         sut.RemoveAttributeMapperFor<int>();
      }

      [Observation]
      public void should_not_be_able_to_retrieve_an_attribute_for_the_removed_type()
      {
         The.Action(() => sut.AttributeMapperFor<int>()).ShouldThrowAn<AttributeMapperNotFoundException>();
      }

      [Observation]
      public void should_be_able_to_add_a_new_attribute_mapper_for_the_type_removed()
      {
         sut.AddAttributeMapper(new IntAttributeMapper<TestSerializationContext>());
      }
   }

   
   public class When_removing_an_attribute_that_does_not_exist : concern_for_xml_attribute_mapper_repository
   {
      protected override void Context()
      {
         base.Context();
         sut.AddAttributeMapper(new IntAttributeMapper<TestSerializationContext>());
      }

      protected override void Because()
      {
         sut.RemoveAttributeMapperFor<bool>();
      }

      [Observation]
      public void should_not_crash()
      {
         sut.RemoveAttributeMapperFor<bool>();
      }
   }
}