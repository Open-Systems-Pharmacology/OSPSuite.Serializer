using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class NullableFloatAttributeMapper<TContext> : NullableAttributeMapper<float?, TContext>
   {
      protected override object ValueFrom(string attributeValue, NumberFormatInfo numberFormatInfo) => float.Parse(attributeValue, numberFormatInfo);

      protected override string ValueFor(float? valueToConvert, NumberFormatInfo numberFormatInfo) => valueToConvert.Value.ToString(numberFormatInfo);
   }
}