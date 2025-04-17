using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class FloatAttributeMapper<TContext> : AttributeMapper<float, TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo = new NumberFormatInfo();

      public override string Convert(float valueToConvert, TContext context) => valueToConvert.ToString(_numberFormatInfo);

      public override object ConvertFrom(string attributeValue, TContext context) => float.Parse(attributeValue, _numberFormatInfo);
   }
}