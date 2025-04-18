using System.Drawing;
using System.Globalization;

namespace OSPSuite.Serializer.Attributes
{
   public class ColorAttributeMapper<TContext> : AttributeMapper<Color, TContext>
   {
      private readonly NumberFormatInfo _numberFormatInfo = new NumberFormatInfo();

      public override string Convert(Color color, TContext context)
      {
         if (color.IsNamedColor)
            return color.Name;

         return color.ToArgb().ToString(_numberFormatInfo);
      }

      public override object ConvertFrom(string attributeValue, TContext context)
      {
         if (string.IsNullOrEmpty(attributeValue))
            return Color.Empty;

         if (int.TryParse(attributeValue, out var argb))
         {
            // Color was saved as ARGB value
            return Color.FromArgb(argb);
         }

         // Color was probably as Named value
         return Color.FromName(attributeValue);
      }
   }
}