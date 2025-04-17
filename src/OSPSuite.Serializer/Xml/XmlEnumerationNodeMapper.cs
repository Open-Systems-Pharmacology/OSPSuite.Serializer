using System.Linq;
using System.Xml.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Xml
{
   internal class XmlEnumerationNodeMapper<TObject, TProperty, TContext> : IXmlMapper<TObject, TContext>
   {
      private readonly IEnumerableMap<TObject, TProperty, TContext> _enumerableMap;
      private readonly IXmlSerializerRepository<TContext> _serializerRepository;
      public string ChildMappingName { get; set; }
      public string MappingName { get; set; }

      public XmlEnumerationNodeMapper(IXmlSerializerRepository<TContext> serializerRepository, IEnumerableMap<TObject, TProperty, TContext> enumerableMap)
      {
         _serializerRepository = serializerRepository;
         _enumerableMap = enumerableMap;
      }

      private bool canBeSerializedAsValue => typeof(TProperty).IsPrimitive || typeof(TProperty) == typeof(string);

      public XObject Serialize(TObject objectToSerialize, TContext context)
      {
         var listNode = _serializerRepository.CreateElement(MappingName);
         var enumeration = _enumerableMap.Enumerate(objectToSerialize);

         //enumeration is null?return node only if empty enumeration should be generated
         if (enumeration == null)
            return _serializerRepository.CreateNodeForEmptyEnumeration ? listNode : null;

         var list = enumeration.ToList();

         //enumeration is empty and empty node should not be generated? returns null
         if (!list.Any() && !_serializerRepository.CreateNodeForEmptyEnumeration)
            return null;

         list.Each(child => listNode.Add(childNodeFor(child, context)));
         return listNode;
      }

      public void Deserialize(TObject objectToDeserialize, XElement element, TContext context)
      {
         if (element == null)
            return;

         var entityListNode = element.Element(MappingName);
         if (entityListNode == null)
            return;

         if (_enumerableMap.AddFunction == null)
            throw new EnumerableMappingException(_enumerableMap);

         var addFunction = _enumerableMap.AddFunction(objectToDeserialize, context);

         foreach (var childElement in entityListNode.Elements())
         {
            addFunction(valueFromChildNode(childElement, context));
         }
      }

      private XElement childNodeFor(TProperty child, TContext context)
      {
         if (canBeSerializedAsValue)
         {
            var childNode = _serializerRepository.CreateElement(ChildMappingName);
            childNode.SetValue(child.ConvertedTo<string>());
            return childNode;
         }

         var serializer = _serializerRepository.SerializerFor(child);
         return serializer.Serialize(child, context);
      }

      private TProperty valueFromChildNode(XElement childElement, TContext context)
      {
         if (canBeSerializedAsValue)
            return childElement.Value.ConvertedTo<TProperty>();

         var serializer = _serializerRepository.SerializerFor(childElement);
         return serializer.Deserialize<TProperty>(childElement, context);
      }
   }
}