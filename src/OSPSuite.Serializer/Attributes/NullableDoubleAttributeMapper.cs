using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class NullableDoubleAttributeMapper<TContext> : NullableAttributeMapper<double?, TContext>
   {
      protected override object ValueFrom(string attributeValue, NumberFormatInfo numberFormatInfo) => double.Parse(attributeValue, numberFormatInfo);

      protected override string ValueFor(double? valueToConvert, NumberFormatInfo numberFormatInfo) => valueToConvert.Value.ToString(numberFormatInfo);
   }
}