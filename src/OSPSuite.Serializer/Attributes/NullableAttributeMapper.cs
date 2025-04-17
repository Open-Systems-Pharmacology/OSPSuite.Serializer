using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public abstract class NullableAttributeMapper<TNullableType, TContext> : AttributeMapper<TNullableType, TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo = new NumberFormatInfo();

      public override string Convert(TNullableType valueToConvert, TContext context)
      {
         return valueToConvert == null ? null : ValueFor(valueToConvert, _numberFormatInfo);
      }

      public override object ConvertFrom(string attributeValue, TContext context)
      {
         return string.IsNullOrEmpty(attributeValue) ? null : ValueFrom(attributeValue, _numberFormatInfo);
      }

      protected abstract object ValueFrom(string attributeValue, NumberFormatInfo numberFormatInfo);
      protected abstract string ValueFor(TNullableType valueToConvert, NumberFormatInfo numberFormatInfo);
   }
}