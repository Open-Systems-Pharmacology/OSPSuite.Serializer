using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class LongAttributeMapper<TContext> : AttributeMapper<long, TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo = new NumberFormatInfo();

      public override string Convert(long valueToConvert, TContext context) => valueToConvert.ToString(_numberFormatInfo);

      public override object ConvertFrom(string attributeValue, TContext context) => long.Parse(attributeValue, _numberFormatInfo);
   }
}