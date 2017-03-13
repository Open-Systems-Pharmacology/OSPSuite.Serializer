using System.Drawing;
using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class ColorAttributeMapper<TContext> : AttributeMapper<Color, TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo;

      public ColorAttributeMapper()
      {
         _numberFormatInfo = new NumberFormatInfo();
      }

      public override string Convert(Color color, TContext context)
      {
         if (color.IsKnownColor)
            return color.ToKnownColor().ToString();

         return color.ToArgb().ToString(_numberFormatInfo);
      }

      public override object ConvertFrom(string attributeValue, TContext context)
      {
         if (string.IsNullOrEmpty(attributeValue))
            return Color.Empty;

         int argb;
         if (int.TryParse(attributeValue, out argb))
         {
            // Color was saved as ARGB value
            return Color.FromArgb(argb);
         }

         // Color was probably as Named value
         return Color.FromName(attributeValue);
      }
   }
}