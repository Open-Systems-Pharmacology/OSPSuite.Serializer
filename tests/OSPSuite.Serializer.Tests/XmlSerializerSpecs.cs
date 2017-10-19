using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Xml;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_XmlSerializer<T> : ContextSpecification<IXmlSerializer<T, TestSerializationContext>>
   {
      protected Project _projectToSerialize;
      protected IXmlSerializerRepository<TestSerializationContext> _serializerRepository;
      protected IAttributeMapperRepository<TestSerializationContext> _attributeMapperRepository;
      protected static string NameSpace = "http://www.pk-sim.com/SimModelSchema";
      protected TestSerializationContext _context;

      protected override void Context()
      {
         _serializerRepository = new XmlSerializerRepository<TestSerializationContext>();
         _attributeMapperRepository = new AttributeMapperRepository<TestSerializationContext>();
         _attributeMapperRepository.AddDefaultAttributeMappers();
         _projectToSerialize = new Project(2);
         _projectToSerialize.Compound = new Compound("1") {Name = "toto", CompoundType = "acid"};

         _projectToSerialize.Name = "Toto";
         _context = new TestSerializationContext();
      }
   }

   public class When_serializing_an_object_to_xml_containing_simple_properties_that_can_be_serialized : concern_for_XmlSerializer<Project>
   {
      private XElement _result;

      protected override void Context()
      {
         base.Context();
         _serializerRepository.Namespace = NameSpace;
         sut = new ProjectSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(new CompoundSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.PerformMapping();
      }

      protected override void Because()
      {
         _result = sut.Serialize(_projectToSerialize, _context);
      }

      [Test]
      public void should_produce_a_valid_xml()
      {
         var xmlValidator = new XmlValidator(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SchemaSpecs.xsd"), NameSpace);
         var validationResult = xmlValidator.Validate(_result.ToString());
         validationResult.Success.ShouldBeTrue(validationResult.FullMessageLog);
      }
   }

   public class When_serialzing_a_property_to_xml_for_which_no_attribute_mapper_was_defined : concern_for_XmlSerializer<AnotherProject>
   {
      protected override void Context()
      {
         base.Context();
         sut = new CorruptSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(sut);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => _serializerRepository.PerformMapping()).ShouldThrowAn<AttributeMappingException>();
      }
   }

   public class When_serialzing_a_private_member_to_xml_for_which_the_type_was_not_specified_correctly : concern_for_XmlSerializer<AnotherProject>
   {
      protected override void Context()
      {
         base.Context();
         sut = new CorruptSerializerForPrivatePropertyType().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(sut);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.PerformMapping()).ShouldThrowAn<MemberAccessException>();
      }
   }

   public class When_serialzing_a_private_member_to_xml_for_which_the_member_name_was_not_specified_correctly : concern_for_XmlSerializer<AnotherProject>
   {
      protected override void Context()
      {
         base.Context();
         sut = new CorruptSerializerForPrivatePropertyName().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(sut);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.PerformMapping()).ShouldThrowAn<MemberAccessException>();
      }
   }

   public class When_deserializing_a_valid_xml : concern_for_XmlSerializer<Project>
   {
      private XElement _xmlToDeserialize;
      private Project _result;

      protected override void Context()
      {
         base.Context();
         sut = new ProjectSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(new CompoundSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_projectToSerialize, _context);
      }

      protected override void Because()
      {
         _result = sut.Deserialize(_xmlToDeserialize, _context) as Project;
      }

      [Observation]
      public void should_return_a_valid_object()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void the_property_for_the_deserialized_object_should_be_the_same_as_the_one_from_the_original_object()
      {
         AreEquals.Projects(_projectToSerialize, _result);
      }
   }

   public class When_deserializing_a_existing_object_from_a_valid_xml : concern_for_XmlSerializer<Project>
   {
      private XElement _xmlToDeserialize;
      private Project _result;

      protected override void Context()
      {
         base.Context();
         sut = new ProjectSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(new CompoundSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_projectToSerialize, _context);
         _result = new Project();
      }

      protected override void Because()
      {
         sut.Deserialize(_result, _xmlToDeserialize, _context);
      }

      [Observation]
      public void the_property_for_the_deserialized_object_should_be_the_same_as_the_one_from_the_original_object()
      {
         AreEquals.Projects(_projectToSerialize, _result);
      }
   }

   public class When_serializing_an_object_to_xml_with_a_sub_object_for_which_the_type_can_only_be_retrieved_dynamically : concern_for_XmlSerializer<Application>
   {
      private XElement _xmlToDeserialize;
      private Application _applicationToSerialize;
      private Application _result;

      protected override void Context()
      {
         base.Context();
         _applicationToSerialize = new Application {Formulation = new MyFormulation()};
         sut = new XmlApplicationSerializer();
         sut.SetRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(new XmlFormulationSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_applicationToSerialize, _context);
      }

      protected override void Because()
      {
         _result = sut.Deserialize(_xmlToDeserialize, _context) as Application;
      }

      [Observation]
      public void should_return_a_valid_object()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void the_property_for_the_deserialized_object_should_be_the_same_as_the_one_from_the_original_object()
      {
         _result.Id.ShouldBeEqualTo(_applicationToSerialize.Id);
         _result.Formulation.ShouldBeAnInstanceOf<MyFormulation>();
         _result.Formulation.Id.ShouldBeEqualTo(_applicationToSerialize.Formulation.Id);
         _result.IsValid.ShouldBeEqualTo(_applicationToSerialize.IsValid);
      }
   }

   public class When_perfoming_the_mapping_for_an_xml_serializer_registering_a_dynamic_property_more_than_once : concern_for_XmlSerializer<Application>
   {
      protected override void Context()
      {
         base.Context();
         sut = new CorrupApplicationSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(new XmlFormulationSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(sut);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => _serializerRepository.PerformMapping()).ShouldThrowAn<Exception>();
      }
   }

   public class When_perfoming_the_mapping_for_an_xml_serializer_registering_a_dynamic_property_that_is_not_availabe : concern_for_XmlSerializer<Application>
   {
      protected override void Context()
      {
         base.Context();
         sut = new CorrupApplicationSerializer2().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(sut);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => _serializerRepository.PerformMapping()).ShouldThrowAn<Exception>();
      }
   }

   public class When_deserializing_an_object_fron_xml_with_a_serializer_for_which_a_user_defined_creation_method_was_defined : concern_for_XmlSerializer<Compound>
   {
      private Compound _compound;
      private XElement _xmlToDeserialize;
      private object _result;

      protected override void Context()
      {
         base.Context();
         _compound = new Compound();
         sut = new OverrideCreationCompoundSerializer {Compound = _compound}.WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_compound, _context);
      }

      protected override void Because()
      {
         _result = sut.Deserialize(_xmlToDeserialize, _context);
      }

      [Observation]
      public void should_use_the_creation_method_to_create_an_object_and_deserialize_it()
      {
         _result.ShouldBeEqualTo(_compound);
      }
   }

   public class When_deserializing_an_object_from_xml_without_parameter_less_constructor_with_a_serializer_that_does_not_take_over_the_object_creation :
      concern_for_XmlSerializer<ObjectWithoutParameterLessConstructor>
   {
      private ObjectWithoutParameterLessConstructor _objectWithoutParameterLessConstructor;
      private XElement _xmlToDeserialize;

      protected override void Context()
      {
         base.Context();
         _objectWithoutParameterLessConstructor = new ObjectWithoutParameterLessConstructor("tralal");
         sut = new CorruptedObjectWithoutParameterLessConstructorXmlSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_objectWithoutParameterLessConstructor, _context);
      }

      [Observation]
      public void should_throw_a_missing_parameter_exception()
      {
         The.Action(() => sut.Deserialize(_xmlToDeserialize, _context)).ShouldThrowAn<MissingParameterLessConstructorException>();
      }
   }

   public class When_deserializing_an_object_from_xml_without_parameter_less_constructor_with_a_serializer_that_does_take_over_the_object_creation :
      concern_for_XmlSerializer<ObjectWithoutParameterLessConstructor>
   {
      private ObjectWithoutParameterLessConstructor _objectWithoutParameterLessConstructor;
      private XElement _xmlToDeserialize;
      private object _result;

      protected override void Context()
      {
         base.Context();
         _objectWithoutParameterLessConstructor = new ObjectWithoutParameterLessConstructor("tralal");
         sut = new ObjectWithoutParameterLessConstructorXmlSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_objectWithoutParameterLessConstructor, _context);
      }

      protected override void Because()
      {
         _result = sut.Deserialize(_xmlToDeserialize, _context);
      }

      [Observation]
      public void should_use_the_creation_method_to_create_an_object_and_deserialize_it()
      {
         _result.ShouldNotBeNull();
      }
   }

   public class When_serializing_an_object_to_xml_for_which_some_static_map_properties_are_null : concern_for_XmlSerializer<Project>
   {
      private XElement _xmlToDeserialize;
      private Project _result;

      protected override void Context()
      {
         base.Context();
         sut = new ProjectSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _projectToSerialize = new Project(3);
         _projectToSerialize.Compound = null;
         _serializerRepository.AddSerializer(new CompoundSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_projectToSerialize, _context);
      }

      protected override void Because()
      {
         _result = sut.Deserialize(_xmlToDeserialize, _context) as Project;
      }

      [Observation]
      public void should_be_able_to_serialize_and_deserialize_the_object()
      {
         _result.ShouldNotBeNull();
         _result.Compound.ShouldBeNull();
      }
   }

   public class When_deserialzing_an_object_to_xml_for_which_some_dynamic_map_properties_are_null : concern_for_XmlSerializer<Application>
   {
      private XElement _xmlToDeserialize;
      private Application _applicationToSerialize;
      private Application _result;

      protected override void Context()
      {
         base.Context();
         sut = new XmlApplicationSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _applicationToSerialize = new Application {Formulation = null, AnotherFormulation = new MyFormulation()};
         sut = new XmlApplicationSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(new XmlFormulationSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_applicationToSerialize, _context);
      }

      protected override void Because()
      {
         _result = sut.Deserialize(_xmlToDeserialize, _context) as Application;
      }

      [Observation]
      public void should_be_able_to_serialize_and_deserialize_the_object()
      {
         _result.ShouldNotBeNull();
         _result.Formulation.ShouldBeNull();
         _result.AnotherFormulation.ShouldNotBeNull();
      }
   }

   public class When_serializing_an_object_to_xml_containing_an_enumeration : concern_for_XmlSerializer<Container>
   {
      private XElement _xmlToDeserialize;
      private Container _containerToSerialize;
      private Container _result;

      protected override void Context()
      {
         base.Context();
         sut = new XmlContainerSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _containerToSerialize = new Container();
         _containerToSerialize.AddChild(new Entity("entity1"));
         _containerToSerialize.AddChild(new Entity("entity2"));
         _containerToSerialize.AddChild(new Entity("entity3"));
         _serializerRepository.AddSerializer(new XmlEntitySerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_containerToSerialize, _context);
      }

      protected override void Because()
      {
         _result = sut.Deserialize(_xmlToDeserialize, _context) as Container;
      }

      [Observation]
      public void should_be_able_to_serialize_and_deserialize_the_object()
      {
         _result.ShouldNotBeNull();
         var children = _result.Children.ToList();
         children.Count.ShouldBeEqualTo(3);
         children[0].Id.ShouldBeEqualTo("entity1");
         children[1].Id.ShouldBeEqualTo("entity2");
         children[2].Id.ShouldBeEqualTo("entity3");
      }
   }

   public class When_deserializing_an_object_to_xml_containing_an_enumeration_with_a_deserializer_that_does_not_contain_any_add_method : concern_for_XmlSerializer<Container>
   {
      private XElement _xmlToDeserialize;
      private Container _containerToSerialize;

      protected override void Context()
      {
         base.Context();
         sut = new CorruptContainerSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _containerToSerialize = new Container();
         _containerToSerialize.AddChild(new Entity("entity1"));
         _containerToSerialize.AddChild(new Entity("entity2"));
         _containerToSerialize.AddChild(new Entity("entity3"));
         _serializerRepository.AddSerializer(new XmlEntitySerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_containerToSerialize, _context);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Deserialize(_xmlToDeserialize, _context)).ShouldThrowAn<EnumerableMappingException>();
      }
   }

   public class When_serializing_an_object_to_xml_containing_a_user_defined_mapping_ : concern_for_XmlSerializer<Parameter>
   {
      private XElement _xmlToDeserialize;
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         sut = new ParameterSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _parameter = new Parameter();
         _serializerRepository.Namespace = NameSpace;

         _serializerRepository.AddSerializer(sut);
         _serializerRepository.AddSerializer(new StringSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(new ParameterInfoSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(new UnitSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.PerformMapping();

         _xmlToDeserialize = sut.Serialize(_parameter, _context);
      }

      [Observation]
      public void should_be_able_to_match_the_predefined_schema()
      {
         var xmlValidator = new XmlValidator(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SchemaSpecs.xsd"), NameSpace);
         var validationResult = xmlValidator.Validate(_xmlToDeserialize.ToString());
         validationResult.Success.ShouldBeTrue(validationResult.FullMessageLog);
      }
   }

   public class When_serializing_a_complex_object_hiearchy_with_references_and_enumeration_to_the_same_object_type_to_xml : concern_for_XmlSerializer<Model>
   {
      private Model _model;
      private XElement _xmlToDeserialize;
      private Model _result;

      protected override void Context()
      {
         base.Context();
         _model = new Model {Name = "MyModel"};
         var root = new Container("Root");
         root.AddChild(new Entity("MyEntity"));
         root.AddChild(new Container("MySubContainer"));
         _model.Root = root;
         _attributeMapperRepository.ReferenceMapper = new MyReferenceMapper();
         sut = new XmlModelSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.AddSerializer(new XmlEntitySerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(new XmlContainerSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_model, _context);
      }

      protected override void Because()
      {
         _result = sut.Deserialize<Model>(_xmlToDeserialize, _context);
      }

      [Observation]
      public void should_be_able_to_deserialize_the_top_object()
      {
         _result.ShouldNotBeNull();
      }
   }

   public class When_serializing_an_object__to_xml_with_a_reference_and_the_reference_mapper_has_not_been_set : concern_for_XmlSerializer<Model>
   {
      private Model _model;

      protected override void Context()
      {
         base.Context();
         _model = new Model {Name = "MyModel"};
         var root = new Container("Root");
         root.AddChild(new Entity("MyEntity"));
         root.AddChild(new Container("MySubContainer"));
         _model.Root = root;
         sut = new XmlModelSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.AddSerializer(new XmlEntitySerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(new XmlContainerSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
      }

      [Observation]
      public void should_throw_an_exception_while_performing_the_mapping()
      {
         The.Action(() => _serializerRepository.PerformMapping()).ShouldThrowAn<ReferenceMappingNullException>();
      }
   }

   public class When_serializing_an_object_to_xml_with_a_reference_and_the_reference_mapper_has_been_set : concern_for_XmlSerializer<Model>
   {
      private Model _model;
      private XElement _xmlToDeserialize;

      protected override void Context()
      {
         base.Context();
         _model = new Model {Name = "MyModel"};
         var root = new Container("Root");
         root.AddChild(new Entity("MyEntity"));
         root.AddChild(new Container("MySubContainer"));
         _model.Root = root;
         sut = new XmlModelSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.AddSerializer(new XmlEntitySerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.AddSerializer(new XmlContainerSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _attributeMapperRepository.ReferenceMapper = new MyReferenceMapper();
         _serializerRepository.PerformMapping();
      }

      [Observation]
      public void should_be_able_to_serialize_the_entity()
      {
         _xmlToDeserialize = sut.Serialize(_model, _context);
      }
   }

   public class When_serializing_an_object_to_xml_containing_a_collection_of_strings : concern_for_XmlSerializer<OneClassWithStringAndPrimitiveTypeCollection>
   {
      private OneClassWithStringAndPrimitiveTypeCollection _objectToDeserialize;
      private XElement _xmlToDeserialize;
      private OneClassWithStringAndPrimitiveTypeCollection _result;

      protected override void Context()
      {
         base.Context();
         _objectToDeserialize = new OneClassWithStringAndPrimitiveTypeCollection();
         _objectToDeserialize.MyStringProperties.Add("Prop1");
         _objectToDeserialize.MyStringProperties.Add("Prop2");
         _objectToDeserialize.MyDoubleProperties.Add(1.154);
         _objectToDeserialize.MyDoubleProperties.Add(2.156);
         sut = new XmlOneClassWithAStringCollectionSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_objectToDeserialize, _context);
      }

      [Observation]
      public void should_be_able_to_deserialize_the_object()
      {
         _result = sut.Deserialize<OneClassWithStringAndPrimitiveTypeCollection>(_xmlToDeserialize, _context);
         _result.MyStringProperties.ShouldBeEqualTo(_objectToDeserialize.MyStringProperties);
         _result.MyDoubleProperties.ShouldBeEqualTo(_objectToDeserialize.MyDoubleProperties);
      }
   }

   public class When_serializing_an_object_to_xml_that_is_itself_enumerable : concern_for_XmlSerializer<EntityCache>
   {
      private EntityCache _entityCache;
      private XElement _xmlToDeserialize;
      private EntityCache _result;

      protected override void Context()
      {
         base.Context();
         _entityCache = new EntityCache();
         _entityCache.Add(new Entity("1"));
         _entityCache.Add(new Entity("2"));
         _entityCache.Add(new Entity("3"));
         sut = new XmlEntityCacheSerializer().WithRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(sut);
         _serializerRepository.AddSerializer(new XmlEntitySerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _serializerRepository.PerformMapping();
         _xmlToDeserialize = sut.Serialize(_entityCache, _context);
      }

      [Observation]
      public void should_be_able_to_create_sub_nodes_for_each_child_of_the_enumeratin()
      {
         _result = sut.Deserialize<EntityCache>(_xmlToDeserialize, _context);
         _result.Count().ShouldBeEqualTo(_result.Count());
      }
   }
}