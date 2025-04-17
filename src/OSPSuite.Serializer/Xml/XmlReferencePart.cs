using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Extensions;

namespace OSPSuite.Serializer.Xml
{
   internal class XmlReferencePart<TObject, TContext> : MappingPart, IXmlMappingPart<TObject, TContext>
   {
      private readonly IAttributeMapperRepository<TContext> _attributeMapperRepository;

      public XmlReferencePart(IPropertyMap propertyMap, IAttributeMapperRepository<TContext> attributeMapperRepository)
         : base(propertyMap)
      {
         _attributeMapperRepository = attributeMapperRepository;
      }

      public bool IsNode { get; set; }
      public bool IsAttribute { get; set; }

      public IXmlMapper<TObject, TContext> Mapper
      {
         get
         {
            var referenceMapper = _attributeMapperRepository.ReferenceMapper;
            if (referenceMapper == null)
               throw new ReferenceMappingNullException(typeof(TObject), PropertyMap.Name);

            var referenceAttributeMapper = new XmlReferenceAttributeMapper<TObject, TContext>(PropertyMap, referenceMapper)
            {
               MappingName = MappingName ?? PropertyMap.MappingName.ToPascalCase()
            };
            return referenceAttributeMapper;
         }
      }
   }
}