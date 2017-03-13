using System.Xml.Linq;

namespace OSPSuite.Serializer.Xml
{
   internal class XmlAttributeMapper<TObject, TContext> : IXmlMapper<TObject, TContext>
   {
      private readonly IAttributeMapper<TContext> _attributeMapper;
      public string MappingName { get; set; }
      private readonly IPropertyMap _propertyMap;

      public XmlAttributeMapper(IPropertyMap propertyMap, IAttributeMapper<TContext> attributeMapper)
      {
         _propertyMap = propertyMap;
         _attributeMapper = attributeMapper;
      }

      public XObject Serialize(TObject objectToSerialize, TContext context)
      {
         string value = _attributeMapper.Convert(_propertyMap.ResolveValue(objectToSerialize), context);
         if (string.IsNullOrEmpty(value))
            return null;

         return new XAttribute(MappingName, value);
      }

      public void Deserialize(TObject objectToDeserialize, XElement element, TContext context)
      {
         //attrbiute does not exist? do not set any value
         XAttribute attribute = element.Attribute(MappingName);
         if (attribute == null) return;
         _propertyMap.SetValue(objectToDeserialize, _attributeMapper.ConvertFrom(attribute.Value, context));
      }
   }
}