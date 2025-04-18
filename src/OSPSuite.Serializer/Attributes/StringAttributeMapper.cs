namespace OSPSuite.Serializer.Attributes
{
   public class StringAttributeMapper<TContext> : AttributeMapper<string, TContext>
   {
      public override string Convert(string valueToConvert, TContext context) => valueToConvert ?? string.Empty;

      public override object ConvertFrom(string attributeValue, TContext context) =>
         //return the intern string (to prevent having the same references in memory)
         StringInterning.Intern(attributeValue);
   }
}