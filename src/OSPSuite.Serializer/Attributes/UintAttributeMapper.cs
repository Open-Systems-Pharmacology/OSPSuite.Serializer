using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class UintAttributeMapper<TContext> : AttributeMapper<uint, TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo = new NumberFormatInfo();

      public override string Convert(uint valueToConvert, TContext context) => valueToConvert.ToString(_numberFormatInfo);

      public override object ConvertFrom(string attributeValue, TContext context) => uint.Parse(attributeValue, _numberFormatInfo);
   }
}