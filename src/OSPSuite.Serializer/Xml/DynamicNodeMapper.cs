using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;

namespace OSPSuite.Serializer.Xml
{
   internal class DynamicNodeMapper<TObject, TContext> : IXmlMapper<TObject, TContext>
   {
      private readonly IPropertyMap _propertyMap;
      private readonly IXmlSerializerRepository<TContext> _serializerRepository;

      public DynamicNodeMapper(IXmlSerializerRepository<TContext> serializerRepository, IPropertyMap propertyMap)
      {
         _serializerRepository = serializerRepository;
         _propertyMap = propertyMap;
      }

      public string MappingName { get; set; }

      public XObject Serialize(TObject objectToSerialize, TContext context)
      {
         var subObject = _propertyMap.ResolveValue(objectToSerialize);
         if (subObject == null) 
            return null;

         var serializer = _serializerRepository.SerializerFor(subObject);
         var subNode = serializer.Serialize(subObject, context);

         if (_serializerRepository.NeedsDeserialization)
            subNode.AddAttribute(SerializerConstants.DynamicName, _propertyMap.Name);

         return subNode;
      }

      public void Deserialize(TObject objectToDeserialize, XElement element, TContext context)
      {
         var subNode = element.GetFirstElementWithAttribute(SerializerConstants.DynamicName, _propertyMap.Name);
         if (subNode == null) return;
         var serializer = _serializerRepository.SerializerFor(subNode);
         if (serializer == null)
            return;

         _propertyMap.SetValue(objectToDeserialize, serializer.Deserialize(subNode, context));
      }
   }
}