using System;

namespace OSPSuite.Serializer.Attributes
{
   public class EnumAttributeMapper<TEnum,TContext> : AttributeMapper<TEnum, TContext>
   {
      public EnumAttributeMapper()
      {
         if (typeof (TEnum).IsEnum) return;

         throw new ArgumentException($"'{typeof(TEnum)}' is not an enum type and cannot be used with the EnumAttributeMapper");
      }

      public override object ConvertFrom(string attributeValue, TContext context)
      {
         return Enum.Parse(typeof (TEnum), attributeValue);
      }

      public override string Convert(TEnum valueToConvert, TContext context)
      {
         return valueToConvert.ToString();
      }
   }
}