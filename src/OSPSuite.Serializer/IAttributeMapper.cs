using System;

namespace OSPSuite.Serializer
{
   public interface IAttributeMapper
   {
      Type ObjectType { get; }
      bool IsMatch(Type attributeType);
      string Convert(object valueToConvert, object context);
      object ConvertFrom(string attributeValue, object context);
   }

   public interface IAttributeMapper<in TContext> : IAttributeMapper
   {
      object ConvertFrom(string attributeValue, TContext context);
   }

   public interface IAttributeMapper<in TAttribute, in TContext> : IAttributeMapper<TContext>
   {
      string Convert(TAttribute valueToConvert, TContext context);
   }
}