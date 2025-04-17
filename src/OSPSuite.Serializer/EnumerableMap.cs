using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Serializer
{
   public interface IEnumerableMap : IPropertyMap
   {
      Type ObjectType { get; }
   }

   public interface IEnumerableMap<TObject, TProperty, TContext> : IEnumerableMap, IPropertyMap<TProperty>
   {
      Func<TObject, TContext, Action<TProperty>> AddFunction { get; set; }
      IEnumerable<TProperty> Enumerate(TObject source);
   }

   internal class EnumerableMap<TObject, TProperty, TContext> : IEnumerableMap<TObject, TProperty, TContext>
   {
      private readonly Func<TObject, IEnumerable<TProperty>> _enumerable;

      public EnumerableMap(Expression<Func<TObject, IEnumerable<TProperty>>> expression)
      {
         Name = expression.Name();
         MappingName = Name;
         _enumerable = expression.Compile();
      }

      public Func<TObject, TContext, Action<TProperty>> AddFunction { get; set; }
      public string MappingName { get; set; }
      public string Name { get; }

      public IEnumerable<TProperty> Enumerate(TObject source)
      {
         return _enumerable(source);
      }

      public Type ObjectType => typeof(TObject);

      public object ResolveValue(object source) => Enumerate(source.DowncastTo<TObject>());

      public void SetValue(object destination, object valueToSet)
      {
         //nothing to do here
      }

      public Type PropertyType => typeof(TProperty);
   }
}