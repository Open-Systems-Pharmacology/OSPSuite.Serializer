using System;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer.Attributes
{
   public class DateTimeAttributeMapper<TContext> : AttributeMapper<DateTime, TContext>
   {
      public override string Convert(DateTime dateValue, TContext context) => dateValue.ToBinary().ConvertedTo<string>();

      public override object ConvertFrom(string attributeValue, TContext context) => DateTime.FromBinary(attributeValue.ConvertedTo<long>());
   }
}