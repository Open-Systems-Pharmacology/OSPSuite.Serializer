using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_XmlSerializerRepository : ContextSpecification<IXmlSerializerRepository<TestSerializationContext>>
   {
      protected IXmlSerializer<Project,TestSerializationContext> _serializerProject;
      protected IXmlSerializer<Compound,TestSerializationContext> _serializerCompound;

      protected override void Context()
      {
         _serializerProject = A.Fake<IXmlSerializer<Project,TestSerializationContext>>();
         A.CallTo(()=>_serializerProject.Name).Returns("Project");
         A.CallTo(() => _serializerProject.ObjectType).Returns(typeof(Project));
         _serializerCompound = A.Fake<IXmlSerializer<Compound,TestSerializationContext>>();
         A.CallTo(() => _serializerCompound.Name).Returns("Compound");
         A.CallTo(() => _serializerCompound.ObjectType).Returns(typeof(Compound));
         sut = new XmlSerializerRepository<TestSerializationContext>();

         sut.AddSerializer(_serializerProject);
         sut.AddSerializer(_serializerCompound);
      }
   }


   public class When_asked_to_retrieve_a_serializer_for_a_given_type : concern_for_XmlSerializerRepository
   {
      [Observation]
      public void should_return_a_serializer_if_one_was_registered_for_the_type()
      {
         sut.SerializerFor<Project>().ShouldBeEqualTo(_serializerProject);
      }

      [Observation]
      public void should_return_null_if_the_requested_type_was_not_registered()
      {
         The.Action(() => sut.SerializerFor<Individual>()).ShouldThrowAn<SerializerNotFoundException>();
      }
   }


   public class When_asked_to_retrieve_a_serializer_for_a_given_instance : concern_for_XmlSerializerRepository
   {
      [Observation]
      public void should_return_a_serializer_if_one_was_registered_for_the_type()
      {
         sut.SerializerFor(new Project()).ShouldBeEqualTo(_serializerProject);
      }

      [Observation]
      public void should_return_null_if_the_requested_type_was_not_registered()
      {
         The.Action(() => sut.SerializerFor(new Individual("toto"))).ShouldThrowAn<SerializerNotFoundException>();
      }
   }


   public class When_asked_to_validate_a_mapping : concern_for_XmlSerializerRepository
   {
      protected override void Because()
      {
         sut.PerformMapping();
      }

      [Observation]
      public void should_iterate_through_all_registered_mapper_and_leverage_the_perform_mapping_action()
      {
         A.CallTo(()=>_serializerCompound.PerformMapping()).MustHaveHappened();
         A.CallTo(() => _serializerProject.PerformMapping()).MustHaveHappened();
      }

      [Observation]
      public void should_iterate_through_all_registered_mapper_and_build_the_mapping()
      {
         A.CallTo(() => _serializerCompound.Build()).MustHaveHappened();
         A.CallTo(() => _serializerProject.Build()).MustHaveHappened();
      }
   }


   public class When_retrieving_a_serializer_for_a_type_for_which_the_serializer_resolution_is_ambiguous : concern_for_XmlSerializerRepository
   {
      protected override void Context()
      {
         var containerSerializer = A.Fake<IXmlSerializer<Container,TestSerializationContext>>();
         A.CallTo(() => containerSerializer.Name).Returns("Container");
         A.CallTo(() => containerSerializer.ObjectType).Returns(typeof(Container));


         var container2Serializer = A.Fake<IXmlSerializer<IContainer2,TestSerializationContext>>();
         A.CallTo(() => container2Serializer.Name).Returns("Container2");
         A.CallTo(() => container2Serializer.ObjectType).Returns(typeof(IContainer2));

         sut = new XmlSerializerRepository<TestSerializationContext>();

         sut.AddSerializer(containerSerializer);
         sut.AddSerializer(container2Serializer);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.SerializerFor<Project>()).ShouldThrowAn<AmbiguousSerializerException>();
      }
   }


   public class When_retrieving_a_serializer_for_a_type_for_which_the_serializer_resolution_needs_to_be_simplified : concern_for_XmlSerializerRepository
   {
      private IXmlSerializer<TestSerializationContext> _containerSerializer;

      protected override void Context()
      {
         _containerSerializer = A.Fake<IXmlSerializer<Container,TestSerializationContext>>();
         A.CallTo(() => _containerSerializer.Name).Returns("Container");
         A.CallTo(() => _containerSerializer.ObjectType).Returns(typeof(Container));
         

         var entitySerializer = A.Fake<IXmlSerializer<Entity,TestSerializationContext>>();
         A.CallTo(() => entitySerializer.Name).Returns("Entity");
         A.CallTo(() => entitySerializer.ObjectType).Returns(typeof(Entity));

         sut = new XmlSerializerRepository<TestSerializationContext>();

         sut.AddSerializer(_containerSerializer);
         sut.AddSerializer(entitySerializer);
      }

      [Observation]
      public void should_use_the_serializer_defined_for_first_object_in_the_hierarchy()
      {
         sut.SerializerFor<Project>().ShouldBeEqualTo(_containerSerializer);
      }
   }



   public class When_retrieving_a_serializer_for_a_type_that_was_not_registered_but_for_which_a_serializer_for_its_base_type_exists : concern_for_XmlSerializerRepository
   {
      private IXmlSerializer<Container,TestSerializationContext> _containerSerializer;

      protected override void Context()
      {
         _containerSerializer = A.Fake<IXmlSerializer<Container,TestSerializationContext>>();
         A.CallTo(() => _containerSerializer.Name).Returns("Tralala");
         A.CallTo(() => _containerSerializer.ObjectType).Returns(typeof(Container));

         sut = new XmlSerializerRepository<TestSerializationContext>();
         sut.AddSerializer(_containerSerializer);
      }

      [Observation]
      public void should_use_the_serializer_defined_for_the_base_type()
      {
         sut.SerializerFor<Project>().ShouldBeEqualTo(_containerSerializer);
      }
   }


   public class When_retrieving_the_list_of_all_serializer_defined_for_a_given_type : concern_for_XmlSerializerRepository
   {
      private IXmlSerializer<Container,TestSerializationContext> _containerSerializer;
      private IXmlSerializer<Project,TestSerializationContext> _projectSerializer;
      private IXmlSerializer<IEntity,TestSerializationContext> _entitySerializer;
      private IXmlSerializer<ObjectWithoutParameterLessConstructor,TestSerializationContext> _notEntitySerializer;

      protected override void Context()
      {
         _containerSerializer = A.Fake<IXmlSerializer<Container,TestSerializationContext>>();
         A.CallTo(() => _containerSerializer.Name).Returns("Container");
         A.CallTo(() => _containerSerializer.ObjectType).Returns(typeof(Container));
         
         _projectSerializer = A.Fake<IXmlSerializer<Project,TestSerializationContext>>();
         A.CallTo(() => _projectSerializer.Name).Returns("Project");
         A.CallTo(() => _projectSerializer.ObjectType).Returns(typeof(Project));

         _entitySerializer = A.Fake<IXmlSerializer<IEntity,TestSerializationContext>>();
         A.CallTo(() => _entitySerializer.Name).Returns("Entity");
         A.CallTo(() => _entitySerializer.ObjectType).Returns(typeof(IEntity));

         _notEntitySerializer = A.Fake<IXmlSerializer<ObjectWithoutParameterLessConstructor,TestSerializationContext>>();
         A.CallTo(() => _notEntitySerializer.Name).Returns("Application");
         A.CallTo(() => _notEntitySerializer.ObjectType).Returns(typeof(ObjectWithoutParameterLessConstructor));

         sut = new XmlSerializerRepository<TestSerializationContext>();
         sut.AddSerializer(_containerSerializer);
         sut.AddSerializer(_projectSerializer);
         sut.AddSerializer(_entitySerializer);
         sut.AddSerializer(_notEntitySerializer);
      }

      [Observation]
      public void should_return_the_list_of_all_registered_serializer_for_the_type()
      {
         sut.AllPossibleSerializerFor(typeof(IEntity)).ShouldOnlyContain(_containerSerializer,_projectSerializer,_entitySerializer);
      } 
   }


   public class When_adding_two_serializer_with_the_same_key_to_the_repository : concern_for_XmlSerializerRepository
   {
      private IXmlSerializer<Container,TestSerializationContext> _serializer2;

      protected override void Context()
      {
         var serializer1 = A.Fake<IXmlSerializer<Container,TestSerializationContext>>();
         A.CallTo(() => serializer1.Name).Returns("toto");

         _serializer2 = A.Fake<IXmlSerializer<Container,TestSerializationContext>>();
         A.CallTo(() => _serializer2.Name).Returns("toto");

         sut = new XmlSerializerRepository<TestSerializationContext>();
         sut.AddSerializer(serializer1);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.AddSerializer(_serializer2)).ShouldThrowAn<Exception>();
      }
   }


   public class When_removing_an_existing_serializer_before_the_mapping_was_performed : concern_for_XmlSerializerRepository
   {
      protected override void Because()
      {
         sut.RemoveSerializer(_serializerProject.Name);
      }
      [Observation]
      public void should_have_removed_the_repository()
      {
         sut.SerializerByKey(_serializerProject.Name).ShouldBeNull();
      }
   }

   public class When_removing_an_existing_serializer_after_the_mapping_was_performed : concern_for_XmlSerializerRepository
   {
      protected override void Context()
      {
         base.Context();
         sut.PerformMapping();
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.RemoveSerializer(_serializerProject.Name)).ShouldThrowAn<Exception>();
      }
   }

   public class When_removing_a_serializer_that_does_not_exist : concern_for_XmlSerializerRepository
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.RemoveSerializer("trala")).ShouldThrowAn<Exception>();
      }
   }
}