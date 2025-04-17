using System;
using System.Xml.Linq;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Serializer.Tests
{
   public class XmlEntitySerializer : XmlSerializer<Entity, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
      }
   }

   public class XmlEntityCacheSerializer : XmlSerializer<EntityCache, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x, x => x.Add).WithMappingName("Elements");
      }
   }

   public class XmlContainerSerializer : XmlSerializer<Container, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         MapEnumerable(x => x.Children, x => x.AddChild);
      }
   }

   public class CorruptContainerSerializer : XmlSerializer<Container, TestSerializationContext>
   {
      public CorruptContainerSerializer()
         : base("CorruptContainerSerializer")
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Id);
         MapEnumerable(x => x.Children);
      }
   }

   public class XmlOneClassWithAStringCollectionSerializer : XmlSerializer<OneClassWithStringAndPrimitiveTypeCollection, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.MyStringProperties, addStringProperties).WithChildMappingName("MyStringProperty");

         //child name not defined
         MapEnumerable(x => x.MyDoubleProperties, addDoubleProperties);
      }

      private Action<double> addDoubleProperties(OneClassWithStringAndPrimitiveTypeCollection myObject)
      {
         return myObject.MyDoubleProperties.Add;
      }

      private Action<string> addStringProperties(OneClassWithStringAndPrimitiveTypeCollection myObject)
      {
         return myObject.MyStringProperties.Add;
      }
   }

   public class ProjectSerializer : XmlSerializer<Project, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.AutoPropertyReadOnlyNoSetter);
         Map(x => x.Id);
         Map(x => x.Name);
         MapPrivate<int>("_version");
         Map(x => x.Compound);
      }
   }

   public class CompoundSerializer : XmlSerializer<Compound, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Name);
         Map(x => x.CompoundType);
      }
   }

   public class XmlModelSerializer : XmlSerializer<Model, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
         Map(x => x.Root);
         MapReference(x => x.Entity).WithMappingName("toto");
      }
   }

   public class StringSerializer : XmlSerializer<string, TestSerializationContext>
   {
      public override void PerformMapping()
      {
      }

      protected override XElement TypedSerialize(string objectToSerialize, TestSerializationContext context)
      {
         var element = SerializerRepository.CreateElement(ElementName);
         element.SetValue(objectToSerialize);
         return element;
      }

      protected override void TypedDeserialize(string objectToDeserialize, XElement outputToDeserialize, TestSerializationContext context)
      {
      }
   }

   public class MyReferenceMapper : IReferenceMapper<TestSerializationContext>
   {
      public string ReferenceFor(object valueToConvert, TestSerializationContext context)
      {
         var entity = valueToConvert as Entity;
         if (entity != null)
            return entity.Id;
         return string.Empty;
      }

      public void ResolveReference(object objectToDeserialize, IPropertyMap propertyMap, string referenceValue, TestSerializationContext serializationContext)
      {
         //nothing to do
      }
   }

   public class OverrideCreationCompoundSerializer : XmlSerializer<Compound, TestSerializationContext>
   {
      public OverrideCreationCompoundSerializer()
         : base("OverrideCreationCompoundSerializer")
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Name);
         Map(x => x.CompoundType);
      }

      public override Compound CreateObject(XElement element, TestSerializationContext context)
      {
         return Compound;
      }

      public Compound Compound { get; set; }
   }

   public class CorruptSerializer : XmlSerializer<AnotherProject, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Name);
         Map(x => x.Individual).AsAttribute();
      }
   }

   public class UnitSerializer : XmlSerializer<Unit, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
         Map(x => x.Color);
      }
   }

   public class ParameterSerializer : XmlSerializer<Parameter, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.DisplayName).AsNode();
         Map(x => x.ParameterInfo).WithMappingName("InfoParam");
         MapEnumerable(x => x.Units).WithMappingName("UnitList");
      }
   }

   public class ParameterInfoSerializer : XmlSerializer<ParameterInfo, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.Description).AsNode().WithMappingName("TheDescription");
      }
   }

   public class CorruptSerializerForPrivatePropertyType : XmlSerializer<AnotherProject, TestSerializationContext>
   {
      public CorruptSerializerForPrivatePropertyType()
         : base("CorruptSerializerForPrivatePropertyType")
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Name);
         MapPrivate<string>("_version");
      }
   }

   public class CorruptSerializerForPrivatePropertyName : XmlSerializer<AnotherProject, TestSerializationContext>
   {
      public CorruptSerializerForPrivatePropertyName()
         : base("CorruptSerializerForPrivatePropertyName")
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Name);
         MapPrivate<int>("tralala");
      }
   }

   public class XmlFormulationSerializer : XmlSerializer<MyFormulation, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
      }
   }

   public class XmlApplicationSerializer : XmlSerializer<Application, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Formulation);
         Map(x => x.AnotherFormulation);
         Map(x => x.IsValid);
      }
   }

   public class CorrupApplicationSerializer : XmlSerializer<Application, TestSerializationContext>
   {
      public CorrupApplicationSerializer()
         : base("CorrupApplicationSerializer")
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Formulation);
         Map(x => x.Formulation);
      }
   }

   public class CorrupApplicationSerializer2 : XmlSerializer<Application, TestSerializationContext>
   {
      public CorrupApplicationSerializer2()
         : base("CorrupApplicationSerializer2")
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Formulation);
      }
   }

   public class ObjectWithoutParameterLessConstructorXmlSerializer : XmlSerializer<ObjectWithoutParameterLessConstructor, TestSerializationContext>
   {
      public override void PerformMapping()
      {
         Map(x => x.OneStringAttribute);
      }

      public override ObjectWithoutParameterLessConstructor CreateObject(XElement element, TestSerializationContext context)
      {
         return new ObjectWithoutParameterLessConstructor("tralala");
      }
   }

   public class CorruptedObjectWithoutParameterLessConstructorXmlSerializer : XmlSerializer<ObjectWithoutParameterLessConstructor, TestSerializationContext>
   {
      public CorruptedObjectWithoutParameterLessConstructorXmlSerializer()
         : base("CorruptedObjectWithoutParameterLessConstructorXmlSerializer")
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.OneStringAttribute);
      }
   }
}