using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OSPSuite.Serializer.Reflection;

namespace OSPSuite.Serializer
{
   public interface IPropertyMapFactory
   {
      IPropertyMap<TProperty> CreateFor<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression);
      IEnumerableMap<TObject, TProperty, TContext> CreateFor<TObject, TProperty, TContext>(Expression<Func<TObject, IEnumerable<TProperty>>> expression);
      IPropertyMap<TProperty> CreateFor<TObject, TProperty>(string memberName);
   }

   internal class PropertyMapFactory : IPropertyMapFactory
   {
      private readonly IMappingNameResolver _mappingNameResolver;
      private readonly IMemberAccessorFactory _memberAccessorFactory;

      public PropertyMapFactory() : this(new MemberAccessorFactory(), new MappingNameResolver())
      {
      }

      public PropertyMapFactory(IMemberAccessorFactory memberAccessorFactory, IMappingNameResolver mappingNameResolver)
      {
         _memberAccessorFactory = memberAccessorFactory;
         _mappingNameResolver = mappingNameResolver;
      }

      public IPropertyMap<TProperty> CreateFor<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
      {
         var propertyMap = new PropertyMap<TProperty>(_memberAccessorFactory.CreateFor(expression));
         propertyMap.MappingName = _mappingNameResolver.MappingNameFor(propertyMap.Name);
         return propertyMap;
      }

      public IEnumerableMap<TObject, TProperty, TContext> CreateFor<TObject, TProperty, TContext>(Expression<Func<TObject, IEnumerable<TProperty>>> expression)
      {
         return new EnumerableMap<TObject, TProperty, TContext>(expression);
      }

      public IPropertyMap<TProperty> CreateFor<TObject, TProperty>(string memberName)
      {
         var propertyMap = new PropertyMap<TProperty>(_memberAccessorFactory.CreateFor<TObject, TProperty>(memberName));
         propertyMap.MappingName = _mappingNameResolver.MappingNameFor(propertyMap.Name);
         return propertyMap;
      }
   }
}