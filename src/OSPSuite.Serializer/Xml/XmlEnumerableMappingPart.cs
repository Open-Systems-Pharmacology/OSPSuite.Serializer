using System;
using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Serializer.Xml
{
   public interface IXmlEnumerableMappingPart<TObject, TProperty, TContext> : IXmlMappingPart<TObject, TContext>, IEnumerableMappingPart<TObject, TProperty, TContext>
   {
   }

   internal class XmlEnumerableMappingPart<TObject, TProperty, TContext> : XmlMappingPart<TObject, TContext>, IXmlEnumerableMappingPart<TObject, TProperty, TContext>
   {
      private readonly IEnumerableMap<TObject, TProperty, TContext> _enumerableMap;

      public XmlEnumerableMappingPart(IEnumerableMap<TObject, TProperty, TContext> enumerableMap, IXmlSerializerRepository<TContext> serializerRepository, IAttributeMapperRepository<TContext> attributeMapperRepository)
         : base(enumerableMap, serializerRepository, attributeMapperRepository)
      {
         _enumerableMap = enumerableMap;
      }

      public string ChildMappingName { get; set; }

      public Func<TObject, TContext, Action<TProperty>> AddFunction
      {
         set { _enumerableMap.AddFunction = value; }
      }

      public override IXmlMapper<TObject, TContext> Mapper
      {
         get
         {
            var nodeMapper = new XmlEnumerationNodeMapper<TObject, TProperty, TContext>(_serializerRepository, _enumerableMap);
            nodeMapper.MappingName = MappingName ?? PropertyMap.MappingName;
            nodeMapper.ChildMappingName = ChildMappingName ?? string.Format("{0}Child", nodeMapper.MappingName);
            return nodeMapper;
         }
      }
   }
}