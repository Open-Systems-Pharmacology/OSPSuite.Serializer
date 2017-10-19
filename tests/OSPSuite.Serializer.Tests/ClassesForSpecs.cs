using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Serializer.Tests
{
   public enum CompoundType
   {
      Acid,
      Base,
      Neutral,
   }

   [Flags]
   public enum FlagEnumType
   {
      None = 0,
      Tall = 2 << 0,
      Fat = 2 << 1,
      FatAndTall = Tall | Fat
   }

   public interface IEntity
   {
      string Id { get; }
   }

   public class Entity : IEntity
   {
      private readonly string _id;

      public Entity() : this("0")
      {
      }

      public Entity(string id)
      {
         _id = id;
      }

      public string Id
      {
         get { return _id; }
      }
   }

   public class Model
   {
      public string Name { get; set; }
      public Container Root { get; set; }
      public Entity Entity { get; set; }
   }

   public interface  IContainer2
   {
      
   }
   public class Container : Entity
   {
      private readonly IList<IEntity> _allChildren = new List<IEntity>();
      public string ContainerType { get; set; }

      public Container(string id) : base(id)
      {
      }

      public Container()
      {
      }

      public IEnumerable<IEntity> Children
      {
         get { return _allChildren; }
      }

      public void AddChild(IEntity child)
      {
         _allChildren.Add(child);
      }
   }

   public class Project : Container, IProject, IContainer2
   {
      private readonly int _version = 3;
      protected readonly int _protectedReadOnlyField = 3;
      protected int _protectedField = 3;
      public string AutoPropertyProtectedSet { get; private set; }

      public string Name { get; set; }
      public string Description;

      public string AutoPropertyReadOnly { get; private set; }
      public string AutoPropertyReadOnlyNoSetter { get;  }

      private string m_ProjectPath = "";

      public string ProjectPath
      {
         get { return m_ProjectPath; }
      }

      private string m_projectManager = "";
      private string lowcaseProperty = "abc";

      public string ProjectManager
      {
         get { return m_projectManager; }
      }

      public string LowCaseProperty
      {
         get { return lowcaseProperty; }
      }

      public string PropertyWithoutBackingField
      {
         get
         {
            AutoPropertyReadOnly = "40";
            return "5";
         }
      }

      public string DoSomething()
      {
         return "5";
      }

      public Individual Individual { get; set; }
      public Compound Compound { get; set; }

      public IList<Individual> AllIndividuals { get; set; }

      public Project(int version) : base(Guid.NewGuid().ToString())
      {
         _version = version;
         AutoPropertyReadOnlyNoSetter = "AutoPropertyReadOnlyNoSetter";
      }

      public Project() : this(0)
      {
      }

      public void AddEntity(Individual entityToAdd)
      {
      }
   }

   public class OneClassWithStringAndPrimitiveTypeCollection
   {
      public IList<string> MyStringProperties { get; private set; }
      public IList<double> MyDoubleProperties { get; private set; }

      public OneClassWithStringAndPrimitiveTypeCollection()
      {
         MyStringProperties = new List<string>();
         MyDoubleProperties = new List<double>();
      }
   }

   public class AnotherProject : Project
   {
   }

   public interface IProject
   {
      Individual Individual { get; set; }
      string AutoPropertyReadOnly { get; }
   }

   public class Individual : Entity
   {
      public Individual(string id) : base(id)
      {
      }
   }

   public class Application : Entity
   {
      public IFormulation Formulation { get; set; }
      public IFormulation AnotherFormulation { get; set; }
      public bool? IsValid { get; set; }
   }

   public class Compound : Entity
   {
      public string Name { get; set; }
      public string CompoundType { get; set; }

      public Compound() : this("2")
      {
      }

      public Compound(string id)
         : base(id)
      {
      }
   }
   public class EntityCache : Cache<string,Entity>
   {
      public EntityCache() : base(x=>x.Id)
      {
      }
   }

   public class ObjectWithoutParameterLessConstructor
   {
      private readonly string _name;

      public ObjectWithoutParameterLessConstructor(string name)
      {
         _name = name;
      }

      public string OneStringAttribute { get; set; }
   }

   public class MyFormulation : Entity, IFormulation
   {
   }

   public class AnotherFormulation : Entity, IFormulation
   {
   }

   public interface IFormulation : IEntity
   {
   }

  

   public class Parameter
   {
      public int Id { get; set; }
      public string DisplayName { get; set; }
      public ParameterInfo ParameterInfo { get; set; }
      public IEnumerable<IUnit> Units { get; set; }

      public Parameter()
      {
         Id = 5;
         DisplayName = "bon jovi";
         ParameterInfo = new ParameterInfo {Description = "tralala"};
         Units = new List<IUnit> {new Unit {Name = "ml"}, new Unit {Name = "cl"}};
      }
   }

   public interface IUnit
   {
      string Name { get; set; }
   }

   public class Unit : IUnit
   {
      public string Name { get; set; }
      public Color Color { get; set; }
   }

   public class ParameterInfo
   {
      public string Description { get; set; }
   }

  
}