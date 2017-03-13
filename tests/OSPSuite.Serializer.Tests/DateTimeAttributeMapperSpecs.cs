using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_date_time_attribute_mapper : ContextSpecification<DateTimeAttributeMapper<TestSerializationContext>>
   {
      protected TestSerializationContext _context;

      protected override void Context()
      {
         sut = new DateTimeAttributeMapper<TestSerializationContext>();
         _context = new TestSerializationContext();
      }
   }

   public class When_serializing_a_date_time : concern_for_date_time_attribute_mapper
   {
      private DateTime _dateToPersist;
      private string _result;

      protected override void Context()
      {
         base.Context();
         _dateToPersist = new DateTime(1979, 05, 24);
      }

      protected override void Because()
      {
         _result = sut.Convert(_dateToPersist, _context);
      }

      [Observation]
      public void should_be_able_to_deserialize_the_serialize_stream()
      {
         sut.ConvertFrom(_result, _context).ShouldBeEqualTo(_dateToPersist);
      }
   }
}