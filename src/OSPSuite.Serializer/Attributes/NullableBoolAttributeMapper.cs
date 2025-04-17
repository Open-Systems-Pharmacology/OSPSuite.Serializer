namespace OSPSuite.Serializer.Attributes
{
   public class NullableBoolAttributeMapper<TContext> : AttributeMapper<bool?, TContext>
   {
      private readonly BoolAttributeMapper<TContext> _booleanAttributeMapper = new BoolAttributeMapper<TContext>();

      public override object ConvertFrom(string attributeValue, TContext context) =>
         string.IsNullOrEmpty(attributeValue) ? null : _booleanAttributeMapper.ConvertFrom(attributeValue, context);

      public override string Convert(bool? valueToConvert, TContext context) =>
         valueToConvert == null ? null : _booleanAttributeMapper.Convert(valueToConvert.Value, context);
   }
}