using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Attributes
{
   public class BoolAttributeMapper<TContext> : AttributeMapper<bool, TContext>
   {
      public override string Convert(bool valueToConvert, TContext context) => valueToConvert ? "1" : "0";

      public override object ConvertFrom(string attributeValue, TContext context)
      {
         if (attributeValue == "1")
            return true;

         if (attributeValue == "0")
            return false;

         return attributeValue.ConvertedTo<bool>();
      }
   }
}