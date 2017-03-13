using System.Xml.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_StaticNodeMapper<TObject> : ContextSpecification<IXmlMapper<TObject, TestSerializationContext>>
   {
      protected IPropertyMap _propertyMap;
      protected IXmlSerializer<TestSerializationContext> _serializer;
      protected IXmlSerializerRepository<TestSerializationContext> _serializerRepository;
      protected TestSerializationContext _context;

      protected override void Context()
      {
         _serializer = A.Fake<IXmlSerializer<TestSerializationContext>>();
         _propertyMap = A.Fake<IPropertyMap>();
         _serializerRepository = A.Fake<IXmlSerializerRepository<TestSerializationContext>>();
         sut = new XmlStaticMapper<TObject, TestSerializationContext>(_serializerRepository, _serializer, _propertyMap);
         _context = new TestSerializationContext();
      }
   }

   public class When_the_static_node_mapper_is_serializing_a_property_of_an_object : concern_for_StaticNodeMapper<Project>
   {
      private Project _project;
      private object _compound;
      private XElement _result;
      private XElement _element;

      protected override void Context()
      {
         base.Context();
         sut.MappingName = string.Empty;
         A.CallTo(() => _propertyMap.Name).Returns("tralala");
         _project = new Project();
         _compound = new object();
         _element = new XElement("titi");
         A.CallTo(() => _propertyMap.ResolveValue(_project)).Returns(_compound);
         A.CallTo(() => _serializer.Serialize(_compound, _context)).Returns(_element);
      }

      protected override void Because()
      {
         _result = (XElement)sut.Serialize(_project, _context);
      }

      [Observation]
      public void should_serialize_the_underlying_property_with_the_help_of_the_provided_serializer()
      {
         _result.ShouldBeEqualTo(_element);
      }
   }

   public class When_the_static_node_mapper_is_deserializing_a_property_of_an_object : concern_for_StaticNodeMapper<Project>
   {
      private Project _project;
      private object _compound;
      private XElement _element;
      private XElement _childElement;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _propertyMap.Name).Returns("tralala");
         _project = new Project();
         _compound = new object();
         _childElement = new XElement("child");
         _element = new XElement("titi", _childElement);
         A.CallTo(() => _propertyMap.ResolveValue(_project)).Returns(_compound);
         sut.MappingName = "child";
         A.CallTo(() => _serializer.Deserialize(_childElement, _context)).Returns(_compound);
      }

      protected override void Because()
      {
         sut.Deserialize(_project, _element, _context);
      }

      [Observation]
      public void should_retrieve_the_sub_node_corresponding_to_the_serializer_and_deserialize_the_property()
      {
         A.CallTo(() => _propertyMap.SetValue(_project, _compound)).MustHaveHappened();
      }
   }
}