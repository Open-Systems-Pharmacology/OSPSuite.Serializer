using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public abstract class NullableAttributeMapper<TNullableType,TContext> : AttributeMapper<TNullableType,TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo;

      protected NullableAttributeMapper()
      {
         _numberFormatInfo = new NumberFormatInfo();
      }

      public override string Convert(TNullableType valueToConvert, TContext context)
      {
         if (valueToConvert == null) return null;
         return ValueFor(valueToConvert, _numberFormatInfo);
      }

      public override object ConvertFrom(string attributeValue, TContext context)
      {
         if (string.IsNullOrEmpty(attributeValue)) return null;
         return ValueFrom(attributeValue, _numberFormatInfo);
      }

      protected abstract object ValueFrom(string attributeValue, NumberFormatInfo numberFormatInfo);
      protected abstract string ValueFor(TNullableType valueToConvert, NumberFormatInfo numberFormatInfo);
   }
}