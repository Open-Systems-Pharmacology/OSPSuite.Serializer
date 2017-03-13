using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class FloatAttributeMapper<TContext> : AttributeMapper<float,TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo;

      public FloatAttributeMapper()
      {
         _numberFormatInfo = new NumberFormatInfo();
      }

      public override string Convert(float valueToConvert, TContext context)
      {
         return valueToConvert.ToString(_numberFormatInfo);
      }

      public override object ConvertFrom(string attributeValue, TContext context)
      {
         return float.Parse(attributeValue, _numberFormatInfo);
      }
   }
}