using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class NullableIntAttributeMapper<TContext> : NullableAttributeMapper<int?,TContext>
   {
      protected override object ValueFrom(string attributeValue, NumberFormatInfo numberFormatInfo)
      {
         return int.Parse(attributeValue, numberFormatInfo);
      }

      protected override string ValueFor(int? valueToConvert, NumberFormatInfo numberFormatInfo)
      {
         return valueToConvert.Value.ToString(numberFormatInfo);
      }
   }
}