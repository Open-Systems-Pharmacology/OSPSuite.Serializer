using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class UintAttributeMapper<TContext> : AttributeMapper<uint,TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo;

      public UintAttributeMapper()
      {
         _numberFormatInfo = new NumberFormatInfo();
      }

      public override string Convert(uint valueToConvert, TContext context)
      {
         return valueToConvert.ToString(_numberFormatInfo);
      }

      public override object ConvertFrom(string attributeValue, TContext context)
      {
         return uint.Parse(attributeValue, _numberFormatInfo);
      }
   }
}