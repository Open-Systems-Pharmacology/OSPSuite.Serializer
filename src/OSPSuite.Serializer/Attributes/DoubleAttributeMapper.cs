using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class DoubleAttributeMapper<TContext> : AttributeMapper<double, TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo = new NumberFormatInfo();

      public override string Convert(double valueToConvert, TContext context) => valueToConvert.ToString(_numberFormatInfo);

      public override object ConvertFrom(string attributeValue, TContext context) => double.Parse(attributeValue, _numberFormatInfo);
   }
}