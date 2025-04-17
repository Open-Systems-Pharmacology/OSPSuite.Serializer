using System;
using OSPSuite.Serializer.Reflection;

namespace OSPSuite.Serializer
{
   public interface IPropertyInfo
   {
      /// <summary>
      ///    Name of member of property mapped
      /// </summary>
      string Name { get; }

      /// <summary>
      ///    Name used for the mapping per default
      /// </summary>
      string MappingName { get; set; }

      /// <summary>
      ///    Type of the mapped property
      /// </summary>
      Type PropertyType { get; }
   }

   public interface IPropertyMap : IPropertyInfo
   {
      /// <summary>
      ///    Returns the value of the given property for the underlying object
      /// </summary>
      /// <param name="source">object for which the value is required</param>
      object ResolveValue(object source);

      /// <summary>
      ///    Sets the value of the given property for the underlying object
      /// </summary>
      /// <param name="destination">object for which the value should be set</param>
      /// <param name="valueToSet">value to set in the object</param>
      void SetValue(object destination, object valueToSet);
   }

   public interface IPropertyMap<TPropertyType> : IPropertyMap
   {
   }

   internal abstract class PropertyMap : IPropertyMap
   {
      private readonly IMemberAccessor _memberAccessor;

      protected PropertyMap(IMemberAccessor memberAccessor)
      {
         _memberAccessor = memberAccessor;
         MappingName = Name;
      }

      public string MappingName { get; set; }

      public abstract Type PropertyType { get; }

      public string Name => _memberAccessor.Name;

      public object ResolveValue(object source) => _memberAccessor.GetValue(source);

      public void SetValue(object destination, object valueToSet) => _memberAccessor.SetValue(destination, valueToSet);
   }

   internal class PropertyMap<TProperty> : PropertyMap, IPropertyMap<TProperty>
   {
      public PropertyMap(IMemberAccessor memberAccessor) : base(memberAccessor)
      {
      }

      public override Type PropertyType => typeof(TProperty);
   }
}