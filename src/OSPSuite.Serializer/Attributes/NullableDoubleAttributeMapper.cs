using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class NullableDoubleAttributeMapper<TContext>: NullableAttributeMapper<double?,TContext>
   {
      protected override object ValueFrom(string attributeValue, NumberFormatInfo numberFormatInfo)
      {
         return double.Parse(attributeValue, numberFormatInfo);
      }

      protected override string ValueFor(double? valueToConvert, NumberFormatInfo numberFormatInfo)
      {
         return valueToConvert.Value.ToString(numberFormatInfo);
      }
   }
}