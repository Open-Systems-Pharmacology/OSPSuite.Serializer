using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class IntAttributeMapper<TContext> : AttributeMapper<int, TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo;

      public IntAttributeMapper()
      {
         _numberFormatInfo = new NumberFormatInfo();
      }

      public override string Convert(int valueToConvert, TContext context)
      {
         return valueToConvert.ToString(_numberFormatInfo);
      }

      public override object ConvertFrom(string attributeValue, TContext context)
      {
         return int.Parse(attributeValue, _numberFormatInfo);
      }
   }
}