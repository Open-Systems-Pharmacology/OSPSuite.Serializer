using System.Xml.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;
using OSPSuite.Serializer.Xml.Extensions;

namespace OSPSuite.Serializer.Tests
{
   internal class concern_for_DynamicNodeMapper : ContextSpecification<DynamicNodeMapper<DynamicObject, TestSerializationContext>>
   {
      private XmlSerializerRepository<TestSerializationContext> _serializerRepository;
      private AttributeMapperRepository<TestSerializationContext> _attributeMapperRepository;
      protected TestSerializationContext _context;
      protected IPropertyMap _propertyMap;

      protected override void Context()
      {
         base.Context();
         _serializerRepository = new XmlSerializerRepository<TestSerializationContext>();
         _attributeMapperRepository = new AttributeMapperRepository<TestSerializationContext>();
         _attributeMapperRepository.AddDefaultAttributeMappers();
         _serializerRepository.AddSerializer(new XmlEntitySerializer().WithRepositories(_serializerRepository, _attributeMapperRepository));
         _context = new TestSerializationContext();
         _propertyMap = A.Fake<IPropertyMap>();
         sut = new DynamicNodeMapper<DynamicObject, TestSerializationContext>(_serializerRepository, _propertyMap);
      }
   }

   internal class When_mapping_dynamic_node_and_the_node_is_unknown : concern_for_DynamicNodeMapper
   {
      private DynamicObject _objectToSerialize;
      private object _subObject;
      private XElement _element;
      private DynamicObject _deserializedObject;

      protected override void Context()
      {
         base.Context();
         _subObject = new Entity();
         _objectToSerialize = new DynamicObject();
         A.CallTo(() => _propertyMap.ResolveValue(_objectToSerialize)).Returns(_subObject);
         A.CallTo(() => _propertyMap.Name).Returns("subObject");
         _element = new XElement("outer");
         var xElement = (XElement)sut.Serialize(_objectToSerialize, _context);
         xElement.Name = "unknown";
         _element.AddElement(xElement);
         _deserializedObject = new DynamicObject();
      }

      protected override void Because()
      {
         sut.Deserialize(_deserializedObject, _element, _context);
      }

      [Observation]
      public void the_property_mapper_should_not_set_the_property()
      {
         A.CallTo(() => _propertyMap.SetValue(_deserializedObject, A<Entity>._)).MustNotHaveHappened();
      }
   }

   internal class When_mapping_dynamic_node : concern_for_DynamicNodeMapper
   {
      private DynamicObject _objectToSerialize;
      private object _subObject;
      private XElement _element;
      private DynamicObject _deserializedObject;

      protected override void Context()
      {
         base.Context();
         _subObject = new Entity();
         _objectToSerialize = new DynamicObject();
         A.CallTo(() => _propertyMap.ResolveValue(_objectToSerialize)).Returns(_subObject);
         A.CallTo(() => _propertyMap.Name).Returns("subObject");
         _element = new XElement("outer");
         _element.AddElement((XElement)sut.Serialize(_objectToSerialize, _context));
         _deserializedObject = new DynamicObject();
      }

      protected override void Because()
      {
         sut.Deserialize(_deserializedObject, _element, _context);
      }

      [Observation]
      public void the_property_mapper_should_set_the_property()
      {
         A.CallTo(() => _propertyMap.SetValue(_deserializedObject, A<Entity>._)).MustHaveHappened();
      }
   }

   internal class DynamicObject;
}