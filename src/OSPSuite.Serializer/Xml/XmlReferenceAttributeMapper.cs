using System.Xml.Linq;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml.Extensions;

namespace OSPSuite.Serializer.Xml
{
   internal class XmlReferenceAttributeMapper<TObject, TContext> : IXmlMapper<TObject, TContext>
   {
      private readonly IPropertyMap _propertyMap;
      private readonly IReferenceMapper<TContext> _referenceMapper;

      public XmlReferenceAttributeMapper(IPropertyMap propertyMap, IReferenceMapper<TContext> referenceMapper)
      {
         _propertyMap = propertyMap;
         _referenceMapper = referenceMapper;
      }

      public string MappingName { get; set; }

      public XObject Serialize(TObject objectToSerialize, TContext context)
      {
         string value = _referenceMapper.ReferenceFor(_propertyMap.ResolveValue(objectToSerialize), context);
         if (string.IsNullOrEmpty(value))
            return null;

         return new XAttribute(MappingName, value);
      }

      public void Deserialize(TObject objectToDeserialize, XElement element, TContext context)
      {
         _referenceMapper.ResolveReference(objectToDeserialize, _propertyMap, element.GetAttribute(MappingName), context);
      }
   }
}