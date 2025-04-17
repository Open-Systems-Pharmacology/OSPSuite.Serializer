using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Extensions;

namespace OSPSuite.Serializer.Xml
{
   public interface IXmlMappingPart : IMappingPart
   {
      bool IsNode { get; set; }
      bool IsAttribute { get; set; }
   }

   public interface IXmlMappingPart<TObject, TContext> : IXmlMappingPart
   {
      IXmlMapper<TObject, TContext> Mapper { get; }
   }

   internal class XmlMappingPart<TObject, TContext> : MappingPart, IXmlMappingPart<TObject, TContext>
   {
      private readonly IAttributeMapperRepository<TContext> _attributeMapperRepository;
      protected readonly IXmlSerializerRepository<TContext> _serializerRepository;

      public XmlMappingPart(IPropertyMap propertyMap, IXmlSerializerRepository<TContext> serializerRepository,
         IAttributeMapperRepository<TContext> attributeMapperRepository)
         : base(propertyMap)
      {
         _serializerRepository = serializerRepository;
         _attributeMapperRepository = attributeMapperRepository;
      }

      public bool IsReference { get; set; }

      public bool IsNode { get; set; }
      public bool IsAttribute { get; set; }

      public virtual IXmlMapper<TObject, TContext> Mapper
      {
         get
         {
            if (IsAttribute)
               return mapperAsAttribute();

            if (IsNode)
               return mapperAsNode();

            return bestMapper();
         }
      }

      private IXmlMapper<TObject, TContext> bestMapper()
      {
         return canBeMappedAsAttribute() ? mapperAsAttribute() : mapperAsNode();
      }

      private IXmlMapper<TObject, TContext> mapperAsNode()
      {
         var serializer = _serializerRepository.SerializerOrDefaultFor(PropertyMap.PropertyType);
         IXmlMapper<TObject, TContext> nodeMapper;
         if (serializer == null)
         {
            var allSerializers = _serializerRepository.AllPossibleSerializerFor(PropertyMap.PropertyType);

            //One serializer => static
            if (allSerializers.Count == 1)
               nodeMapper = new XmlStaticMapper<TObject, TContext>(_serializerRepository, allSerializers[0], PropertyMap);
            //More than one
            else if (allSerializers.Count > 1)
               nodeMapper = new DynamicNodeMapper<TObject, TContext>(_serializerRepository, PropertyMap);
            else
               throw new SerializerNotFoundException(PropertyMap.PropertyType, typeof(TObject));
         }
         else
            nodeMapper = new XmlStaticMapper<TObject, TContext>(_serializerRepository, serializer, PropertyMap);

         nodeMapper.MappingName = MappingName ?? PropertyMap.MappingName;

         return nodeMapper;
      }

      private bool canBeMappedAsAttribute()
      {
         return _attributeMapperRepository.AttributeMapperOrDefaultFor(PropertyMap.PropertyType) != null;
      }

      private IXmlMapper<TObject, TContext> mapperAsAttribute()
      {
         var attributeMapper = _attributeMapperRepository.AttributeMapperOrDefaultFor(PropertyMap.PropertyType);
         if (attributeMapper == null)
            throw new AttributeMappingException(typeof(TObject), PropertyMap.Name);

         return new XmlAttributeMapper<TObject, TContext>(PropertyMap, attributeMapper)
         {
            MappingName = MappingName ?? PropertyMap.MappingName.ToPascalCase()
         };
      }
   }
}