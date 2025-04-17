using System.Xml.Linq;

namespace OSPSuite.Serializer.Xml
{
   internal class XmlStaticMapper<TObject, TContext> : IXmlMapper<TObject, TContext>
   {
      private readonly IPropertyMap _propertyMap;
      private readonly IXmlSerializer<TContext> _serializer;
      private readonly IXmlSerializerRepository<TContext> _serializerRepository;
      public string MappingName { get; set; }

      public XmlStaticMapper(IXmlSerializerRepository<TContext> serializerRepository, IXmlSerializer<TContext> serializer, IPropertyMap propertyMap)
      {
         _serializerRepository = serializerRepository;
         _serializer = serializer;
         _propertyMap = propertyMap;
      }

      public XObject Serialize(TObject objectToSerialize, TContext context)
      {
         var subObject = _propertyMap.ResolveValue(objectToSerialize);
         if (subObject == null)
            return null;

         var element = _serializer.Serialize(subObject, context);
         if (element == null)
            return null;

         if (string.Equals(_serializer.ElementName, MappingName))
            return element;

         element.Name = _serializerRepository.ElementNameFor(MappingName);
         return element;
      }

      public void Deserialize(TObject objectToDeserialize, XElement element, TContext context)
      {
         var subElement = element.Element(MappingName);
         if (subElement == null) return;
         _propertyMap.SetValue(objectToDeserialize, _serializer.Deserialize(subElement, context));
      }
   }
}