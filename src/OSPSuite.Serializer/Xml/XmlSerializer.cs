using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Xml
{
   public abstract class XmlSerializer<TObject, TContext> : IXmlSerializer<TObject, TContext>
   {
      private readonly ICache<string, IXmlMappingPart<TObject, TContext>> _allMappingParts = new Cache<string, IXmlMappingPart<TObject, TContext>>(part => part.ExpressionName);
      private readonly IList<IXmlMapper<TObject, TContext>> _allXmlMappers = new List<IXmlMapper<TObject, TContext>>();
      protected readonly IPropertyMapFactory _propertyMapFactory;
      public IXmlSerializerRepository<TContext> SerializerRepository { protected get; set; }
      public IAttributeMapperRepository<TContext> AttributeMapperRepository { protected get; set; }
      public string ElementName { get; set; }

      protected XmlSerializer() : this(typeof (TObject).Name)
      {
      }

      protected XmlSerializer(string name) : this(name, new PropertyMapFactory())
      {
      }

      internal XmlSerializer(string name, IPropertyMapFactory propertyMapFactory)
      {
         _propertyMapFactory = propertyMapFactory;
         Name = name;
         ElementName = name;
      }

      public abstract void PerformMapping();

      public T Deserialize<T>(XElement outputToDeserialize, TContext context)
      {
         return Deserialize(outputToDeserialize, context).DowncastTo<T>();
      }

      public override bool Equals(object obj)
      {
         var serializer = obj as IXmlSerializer;
         return serializer != null && serializer.Name == Name;
      }

      public override int GetHashCode()
      {
         return Name.GetHashCode();
      }

      public string Name { get; }

      public XElement Serialize(object objectToSerialize, TContext context)
      {
         return TypedSerialize(objectToSerialize.DowncastTo<TObject>(), context);
      }

      public void Deserialize(object objectToDeserialize, XElement outputToDeserialize, TContext context)
      {
         TypedDeserialize(objectToDeserialize.DowncastTo<TObject>(), outputToDeserialize, context);
      }

      public object Deserialize(XElement outputToDeserialize, TContext context)
      {
         TObject objectToDeserialize = CreateObject(outputToDeserialize, context);
         TypedDeserialize(objectToDeserialize, outputToDeserialize, context);
         return objectToDeserialize;
      }

      public IXmlMappingPart<TObject, TContext> Map<TProperty>(Expression<Func<TObject, TProperty>> expression)
      {
         var mappingPart = new XmlMappingPart<TObject, TContext>(_propertyMapFactory.CreateFor(expression), SerializerRepository, AttributeMapperRepository);
         _allMappingParts.Add(mappingPart);
         return mappingPart;
      }

      public IXmlMappingPart<TObject, TContext> MapReference<TProperty>(Expression<Func<TObject, TProperty>> expression)
      {
         var mappingPart = new XmlReferencePart<TObject, TContext>(_propertyMapFactory.CreateFor(expression), AttributeMapperRepository);
         _allMappingParts.Add(mappingPart);
         return mappingPart;
      }

      public IXmlEnumerableMappingPart<TObject, TProperty, TContext> MapEnumerable<TProperty>(Expression<Func<TObject, IEnumerable<TProperty>>> expression)
      {
         var mappingPart = new XmlEnumerableMappingPart<TObject, TProperty, TContext>(_propertyMapFactory.CreateFor<TObject, TProperty, TContext>(expression), SerializerRepository, AttributeMapperRepository);
         _allMappingParts.Add(mappingPart);
         return mappingPart;
      }

      public virtual IXmlMappingPart<TObject, TContext> MapPrivate<TProperty>(string propertyName)
      {
         var mappingPart = new XmlMappingPart<TObject, TContext>(_propertyMapFactory.CreateFor<TObject, TProperty>(propertyName), SerializerRepository, AttributeMapperRepository);
         _allMappingParts.Add(mappingPart);
         return mappingPart;
      }

      public IXmlEnumerableMappingPart<TObject, TProperty, TContext> MapEnumerable<TProperty>(Expression<Func<TObject, IEnumerable<TProperty>>> expression, Func<TObject, Action<TProperty>> addFunction)
      {
         return MapEnumerable(expression, (o, ctx) => addFunction(o));
      }

      public IXmlEnumerableMappingPart<TObject, TProperty, TContext> MapEnumerable<TProperty>(Expression<Func<TObject, IEnumerable<TProperty>>> expression, Func<TObject, TContext, Action<TProperty>> addFunction)
      {
         var mappingPart = MapEnumerable(expression);
         mappingPart.AddFunction = addFunction;
         return mappingPart;
      }

      public void Build()
      {
         _allMappingParts.Each(part => _allXmlMappers.Add(part.Mapper));
      }

      public virtual TObject CreateObject(XElement element, TContext context)
      {
         //default implementation returns an instance of the object 
         return ObjectType.CreateInstance<TObject>();
      }

      public Type ObjectType => typeof (TObject);

      public void SetRepositories(IXmlSerializerRepository<TContext> serializerRepository, IAttributeMapperRepository<TContext> attributeMapperRepository)
      {
         SerializerRepository = serializerRepository;
         AttributeMapperRepository = attributeMapperRepository;
      }

      protected virtual XElement TypedSerialize(TObject objectToSerialize, TContext context)
      {
         var xElement = SerializerRepository.CreateElement(ElementName);

         foreach (var xmlMapper in _allXmlMappers)
         {
            var node = xmlMapper.Serialize(objectToSerialize, context);
            if (node == null) continue;
            xElement.Add(node);
         }

         return xElement;
      }

      protected virtual void TypedDeserialize(TObject objectToDeserialize, XElement outputToDeserialize, TContext context)
      {
         foreach (var xmlMapper in _allXmlMappers)
         {
            xmlMapper.Deserialize(objectToDeserialize, outputToDeserialize, context);
         }
      }
   }
}