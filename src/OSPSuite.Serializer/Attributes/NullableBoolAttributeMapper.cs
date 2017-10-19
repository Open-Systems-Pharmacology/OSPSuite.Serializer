namespace OSPSuite.Serializer.Attributes
{
   public class NullableBoolAttributeMapper<TContext> : AttributeMapper<bool?, TContext>
   {
      private readonly BoolAttributeMapper<TContext> _booleanAttributeMapper;

      public NullableBoolAttributeMapper()
      {
         _booleanAttributeMapper = new BoolAttributeMapper<TContext>();
      }

      public override object ConvertFrom(string attributeValue, TContext context)
      {
         if (string.IsNullOrEmpty(attributeValue))
            return null;

         return _booleanAttributeMapper.ConvertFrom(attributeValue, context);
      }

      public override string Convert(bool? valueToConvert, TContext context)
      {
         if (valueToConvert == null) return null;
         return _booleanAttributeMapper.Convert(valueToConvert.Value, context);
      }
   }
}