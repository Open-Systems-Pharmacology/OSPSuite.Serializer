using System.Linq;
using System.Xml.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Serializer.Tests
{
   internal abstract class concern_for_EnumerationNodeMapper : ContextSpecification<XmlEnumerationNodeMapper<Container, IEntity, TestSerializationContext>>
   {
      protected IEnumerableMap<Container, IEntity, TestSerializationContext> _enumerableMap;
      protected IXmlSerializerRepository<TestSerializationContext> _serializerRepository;
      protected IXmlSerializer<TestSerializationContext> _serializer;
      protected TestSerializationContext _context;

      protected override void Context()
      {
         //Cannot generate mock or stub for IXmlSerializerRepository surely because of XElement...
         _serializer = A.Fake<IXmlSerializer<TestSerializationContext>>();
         _serializerRepository = A.Fake<IXmlSerializerRepository<TestSerializationContext>>();
         A.CallTo(_serializerRepository).WithReturnType<IXmlSerializer<TestSerializationContext>>().Returns(_serializer);
         A.CallTo(() => _serializerRepository.CreateElement(A<string>._))
            .ReturnsLazily(x => new XElement(x.GetArgument<string>(0)));

         _enumerableMap = A.Fake<IEnumerableMap<Container, IEntity, TestSerializationContext>>();
         A.CallTo(() => _serializer.Name).Returns("Serializer");
         sut = new XmlEnumerationNodeMapper<Container, IEntity, TestSerializationContext>(_serializerRepository, _enumerableMap);
         _context = new TestSerializationContext();
      }
   }

   internal class When_the_enumeration_node_mapper_is_asked_to_serialize_an_enumeration : concern_for_EnumerationNodeMapper
   {
      private Container _container;
      private XElement _result;
      private IEntity _entity1;
      private IEntity _entity2;
      private XElement _subNodeEntity1;
      private XElement _subNodeEntity2;

      protected override void Context()
      {
         base.Context();
         _container = new Container();
         _entity1 = A.Fake<IEntity>();
         _entity2 = A.Fake<IEntity>();
         _container.AddChild(_entity1);
         _container.AddChild(_entity2);
         _subNodeEntity1 = new XElement("_entity1");
         _subNodeEntity2 = new XElement("_entity2");

         A.CallTo(() => _serializer.Serialize(_entity1, _context)).Returns(_subNodeEntity1);
         A.CallTo(() => _serializer.Serialize(_entity2, _context)).Returns(_subNodeEntity2);
         A.CallTo(() => _enumerableMap.MappingName).Returns("Children");
         A.CallTo(() => _enumerableMap.Enumerate(_container)).Returns(_container.Children);
         sut.MappingName = "MappingName";
         _serializerRepository.CreateNodeForEmptyEnumeration = true;
      }

      protected override void Because()
      {
         _result = (XElement)sut.Serialize(_container, _context);
      }

      [Observation]
      public void should_return_a_root_node_whose_name_is_set_to_the_name_of_the_mapping()
      {
         _result.Name.ToString().ShouldBeEqualTo(sut.MappingName);
      }

      [Observation]
      public void should_serialize_each_children_and_add_the_corresponding_node_under_the_root_node()
      {
         _result.Elements().Count().ShouldBeEqualTo(2);
         _result.Elements().ElementAt(0).ShouldBeEqualTo(_subNodeEntity1);
         _result.Elements().ElementAt(1).ShouldBeEqualTo(_subNodeEntity2);
      }
   }

   internal class When_generating_a_node_for_an_empty_enumeration_and_the_user_sets_the_generation_for_empty_enumeration_to_false : concern_for_EnumerationNodeMapper
   {
      private Container _container;

      protected override void Context()
      {
         base.Context();
         _serializerRepository.CreateNodeForEmptyEnumeration = false;

         _container = new Container();
         A.CallTo(() => _enumerableMap.MappingName).Returns("Children");
         A.CallTo(() => _enumerableMap.Enumerate(_container)).Returns(_container.Children);
         sut.MappingName = "tralal";
      }

      [Observation]
      public void should_return_null()
      {
         sut.Serialize(_container, _context).ShouldBeNull();
      }
   }

   internal class When_generating_a_node_for_an_empty_enumeration_and_the_user_sets_the_generation_for_empty_enumeration_to_true : concern_for_EnumerationNodeMapper
   {
      private Container _container;

      protected override void Context()
      {
         base.Context();
         _serializerRepository.CreateNodeForEmptyEnumeration = true;

         _container = new Container();
         A.CallTo(() => _enumerableMap.MappingName).Returns("Children");
         A.CallTo(() => _enumerableMap.Enumerate(_container)).Returns(_container.Children);
         sut.MappingName = "tralal";
      }

      [Observation]
      public void should_return_a_node()
      {
         sut.Serialize(_container, _context).ShouldNotBeNull();
      }
   }
}