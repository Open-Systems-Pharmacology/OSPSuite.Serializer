using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Serializer.Xml
{
   public interface IXmlSerializer
   {
      string ElementName { get; set; }
      Type ObjectType { get; }
      string Name { get; }
      void PerformMapping();
      void Build();
   }

   public interface IXmlSerializer<TContext> : IXmlSerializer
   {
      object Deserialize(XElement outputToDeserialize, TContext context);
      void Deserialize(object objectToDeserialize, XElement outputToDeserialize, TContext context);
      T Deserialize<T>(XElement outputToDeserialize, TContext context);
      XElement Serialize(object objectToSerialize, TContext context);
      void SetRepositories(IXmlSerializerRepository<TContext> serializerRepository, IAttributeMapperRepository<TContext> attributeMapperRepository);
   }

   public interface IXmlSerializer<TObject, TContext> : IXmlSerializer<TContext>
   {
      /// <summary>
      ///    Allows client to specify how the object should be created
      /// </summary>
      /// <param name="output">output used to deserialize the object that might be needed to create the object</param>
      /// <param name="context">Serialization context</param>
      /// <returns>an instance of type TObject</returns>
      TObject CreateObject(XElement output, TContext context);

      IXmlMappingPart<TObject, TContext> Map<TProperty>(Expression<Func<TObject, TProperty>> expression);
      IXmlMappingPart<TObject, TContext> MapReference<TProperty>(Expression<Func<TObject, TProperty>> expression);
      IXmlEnumerableMappingPart<TObject, TProperty, TContext> MapEnumerable<TProperty>(Expression<Func<TObject, IEnumerable<TProperty>>> expression);
      IXmlMappingPart<TObject, TContext> MapPrivate<TProperty>(string propertyName);
      IXmlEnumerableMappingPart<TObject, TProperty, TContext> MapEnumerable<TProperty>(Expression<Func<TObject, IEnumerable<TProperty>>> expression, Func<TObject, Action<TProperty>> addFunction);
   }
}