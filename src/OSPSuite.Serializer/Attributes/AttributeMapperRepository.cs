using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Serializer.Attributes
{
   public interface IAttributeMapperRepository<TContext>
   {
      IReferenceMapper<TContext> ReferenceMapper { get; set; }
      void AddAttributeMapper(IAttributeMapper<TContext> attributeMapper);
      IEnumerable<IAttributeMapper<TContext>> All();
      IAttributeMapper<TContext> AttributeMapperFor<T>();
      IAttributeMapper<TContext> AttributeMapperOrDefaultFor<T>();
      IAttributeMapper<TContext> AttributeMapperFor(Type type);
      IAttributeMapper<TContext> AttributeMapperOrDefaultFor(Type type);
      void RemoveAttributeMapperFor<T>();
      void RemoveAttributeMapperFor(Type type);
      void AddDefaultAttributeMappers();
   }

   public class AttributeMapperRepository<TContext> : IAttributeMapperRepository<TContext>
   {
      private readonly List<IAttributeMapper<TContext>> _allAttributes = new List<IAttributeMapper<TContext>>();
      public IReferenceMapper<TContext> ReferenceMapper { get; set; }

      public IEnumerable<IAttributeMapper<TContext>> All()
      {
         return _allAttributes;
      }

      public IAttributeMapper<TContext> AttributeMapperFor<T>()
      {
         return AttributeMapperFor(typeof (T));
      }

      public IAttributeMapper<TContext> AttributeMapperOrDefaultFor<T>()
      {
         return AttributeMapperOrDefaultFor(typeof (T));
      }

      public IAttributeMapper<TContext> AttributeMapperFor(Type type)
      {
         var attributeMapper = AttributeMapperOrDefaultFor(type);
         if (attributeMapper != null)
            return attributeMapper;

         throw new AttributeMapperNotFoundException(type);
      }

      public IAttributeMapper<TContext> AttributeMapperOrDefaultFor(Type type)
      {
         return _allAttributes.FirstOrDefault(attr => attr.IsMatch(type));
      }

      public void RemoveAttributeMapperFor<T>()
      {
         RemoveAttributeMapperFor(typeof (T));
      }

      public void RemoveAttributeMapperFor(Type type)
      {
         if (!containsAttributeFor(type)) return;
         _allAttributes.Remove(AttributeMapperFor(type));
      }

      public void AddAttributeMapper(IAttributeMapper<TContext> attributeMapper)
      {
         if (containsAttributeFor(attributeMapper.ObjectType))
            throw new ArgumentException($"A attribute mapper for type '{attributeMapper.ObjectType}' is already registered in the repository '{GetType()}'.");

         _allAttributes.Add(attributeMapper);
      }

      public void AddDefaultAttributeMappers()
      {
         AddAttributeMapper(new IntAttributeMapper<TContext>());
         AddAttributeMapper(new DoubleAttributeMapper<TContext>());
         AddAttributeMapper(new FloatAttributeMapper<TContext>());
         AddAttributeMapper(new BoolAttributeMapper<TContext>());
         AddAttributeMapper(new StringAttributeMapper<TContext>());
         AddAttributeMapper(new UintAttributeMapper<TContext>());
         AddAttributeMapper(new DateTimeAttributeMapper<TContext>());
         AddAttributeMapper(new ColorAttributeMapper<TContext>());
         AddAttributeMapper(new NullableDoubleAttributeMapper<TContext>());
         AddAttributeMapper(new NullableFloatAttributeMapper<TContext>());
         AddAttributeMapper(new NullableIntAttributeMapper<TContext>());
         AddAttributeMapper(new NullableBoolAttributeMapper<TContext>());
      }

      private bool containsAttributeFor(Type type)
      {
         return AttributeMapperOrDefaultFor(type) != null;
      }
   }
}