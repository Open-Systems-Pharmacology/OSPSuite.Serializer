using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_assembly_serializer_scanner : ContextSpecification<IAssemblySerializerScanner<TestSerializationContext>>
   {
      protected IXmlSerializerRepository<TestSerializationContext> _serializerRepository;

      protected override void Context()
      {
         _serializerRepository = new XmlSerializerRepository<TestSerializationContext>();
         sut = new AssemblySerializerScanner<TestSerializationContext>(_serializerRepository);
      }
   }

   
   public class When_asked_to_register_all_type_for_a_given_assembly_implementing_a_certain_interface : concern_for_assembly_serializer_scanner
   {
      protected override void Because()
      {
         _serializerRepository.AddSerializers(x =>
                                                 {
                                                    x.Implementing<IXmlSerializer>();
                                                    x.InAssemblyContainingType<Project>();
                                                    x.UsingAttributeRepository(new AttributeMapperRepository<TestSerializationContext>());
                                                 });
      }

      [Observation]
      public void should_iterate_through_all_registered_mapper_and_leverage_the_perform_mapping_action()
      {
         _serializerRepository.SerializerFor<IProject>().ShouldBeAnInstanceOf<ProjectSerializer>();
         _serializerRepository.SerializerFor<Entity>().ShouldBeAnInstanceOf<XmlEntitySerializer>();
      }
   }

   public class When_the_interface_implemented_by_the_serializer_was_not_defined : concern_for_assembly_serializer_scanner
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => _serializerRepository.AddSerializers(x =>
                                                                  {
                                                                     x.InAssemblyContainingType<Project>();
                                                                     x.UsingAttributeRepository(new AttributeMapperRepository<TestSerializationContext>());
                                                                  })).ShouldThrowAn<Exception>();
      }
   }

   
   public class When_the_assembly_to_scan_was_not_defined : concern_for_assembly_serializer_scanner
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => _serializerRepository.AddSerializers(x =>
                                                                  {
                                                                     x.Implementing<IXmlSerializer>();
                                                                     x.UsingAttributeRepository(new AttributeMapperRepository<TestSerializationContext>());
                                                                  })).ShouldThrowAn<Exception>();
      }
   }

   
   public class When_the_attribute_repository_was_not_defined : concern_for_assembly_serializer_scanner
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => _serializerRepository.AddSerializers(x =>
                                                                  {
                                                                     x.Implementing<IXmlSerializer>();
                                                                     x.InAssemblyContainingType<Project>();
                                                                  })).ShouldThrowAn<Exception>();
      }
   }
}