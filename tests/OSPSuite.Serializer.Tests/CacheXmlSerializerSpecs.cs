using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_CacheXmlSerializer : ContextSpecification<XmlCacheSerializer<Compound, Unit, TestSerializationContext>>
   {
      protected ICache<Compound, Unit> _cache;
      protected TestSerializationContext _context;

      protected override void Context()
      {
         sut = new XmlCacheSerializer<Compound, Unit, TestSerializationContext>();
         _cache = new Cache<Compound, Unit>();
         var d1 = new Unit { Name = "mg", Color = Color.WhiteSmoke };
         var p1 = new Compound("1") { Name = "toto", CompoundType = "Acid" };

         var d2 = new Unit { Name = "oo", Color = Color.Wheat };
         var p2 = new Compound("2") { Name = "tata", CompoundType = "base" };

         var serializerRepository = new XmlSerializerRepository<TestSerializationContext>();
         var attributeMapperRepository = new AttributeMapperRepository<TestSerializationContext>();
         attributeMapperRepository.AddDefaultAttributeMappers();
         serializerRepository.AddSerializer(new CompoundSerializer().WithRepositories(serializerRepository, attributeMapperRepository));
         serializerRepository.AddSerializer(new UnitSerializer().WithRepositories(serializerRepository, attributeMapperRepository));
         serializerRepository.AddSerializer(sut.WithRepositories(serializerRepository, attributeMapperRepository));

         _cache.Add(p1, d1);
         _cache.Add(p2, d2);

         serializerRepository.PerformMapping();
         _context = new TestSerializationContext();
      }
   }

   public class When_deserializing_a_cache_object : concern_for_CacheXmlSerializer
   {
      private ICache<Compound, Unit> _deserialized;

      protected override void Because()
      {
         var element = sut.Serialize(_cache, _context);
         _deserialized = sut.Deserialize(element, _context).DowncastTo<ICache<Compound, Unit>>();
      }

      [Observation]
      public void should_be_able_to_retrieve_the_cache()
      {
         _deserialized.ShouldNotBeNull();
         _deserialized.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_deserializing_a_cache_object_with_unknown_key : concern_for_CacheXmlSerializer
   {
      private ICache<Compound, Unit> _deserialized;

      protected override void Because()
      {
         var element = sut.Serialize(_cache, _context);
         var newElement = new XElement("KeyValue");
         newElement.AddElement(new XElement("unknown"));
         newElement.AddElement(new XElement("Unit").AddAttribute("name", "mg").AddAttribute("color", "blue"));
         
         element.AddElement(newElement);
         _deserialized = sut.Deserialize(element, _context).DowncastTo<ICache<Compound, Unit>>();
      }

      [Observation]
      public void should_be_able_to_retrieve_the_cache()
      {
         _deserialized.ShouldNotBeNull();
         _deserialized.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_deserializing_a_cache_object_with_unknown_value : concern_for_CacheXmlSerializer
   {
      private ICache<Compound, Unit> _deserialized;

      protected override void Because()
      {
         var element = sut.Serialize(_cache, _context);
         var newElement = new XElement("KeyValue");
         newElement.AddElement(new XElement("Compound")).AddAttribute("id", 3).AddAttribute("name", "something").AddAttribute("compoundType", "base");
         newElement.AddElement(new XElement("unknown"));
         element.AddElement(newElement);
         _deserialized = sut.Deserialize(element, _context).DowncastTo<ICache<Compound, Unit>>();
      }

      [Observation]
      public void should_be_able_to_retrieve_the_cache()
      {
         _deserialized.ShouldNotBeNull();
         _deserialized.Count().ShouldBeEqualTo(2);
      }
   }
}