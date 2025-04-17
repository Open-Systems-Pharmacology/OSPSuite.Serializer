using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_color_attribute_mapper : ContextSpecification<ColorAttributeMapper<TestSerializationContext>>
   {
      protected TestSerializationContext _context;

      protected override void Context()
      {
         sut = new ColorAttributeMapper<TestSerializationContext>();
         _context = new TestSerializationContext();
      }
   }

   public class When_deserializing_a_color_created_from_rgb : concern_for_color_attribute_mapper
   {
      private Color _colorFromRGB;
      private string _serializedString;

      protected override void Context()
      {
         base.Context();
         _colorFromRGB = Color.FromArgb(234, 55, 56);
      }

      protected override void Because()
      {
         _serializedString = sut.Convert(_colorFromRGB, _context);
      }

      [Observation]
      public void should_be_able_to_read_the_serialized_color()
      {
         sut.ConvertFrom(_serializedString, _context).ShouldBeEqualTo(_colorFromRGB);
      }
   }

   public class When_deserializing_a_color_created_from_name : concern_for_color_attribute_mapper
   {
      private Color _colorFromName;
      private string _serializedString;

      protected override void Context()
      {
         base.Context();
         _colorFromName = Color.FromKnownColor(KnownColor.AntiqueWhite);
      }

      protected override void Because()
      {
         _serializedString = sut.Convert(_colorFromName, _context);
      }

      [Observation]
      public void should_be_able_to_read_the_serialized_color()
      {
         sut.ConvertFrom(_serializedString, _context).ShouldBeEqualTo(_colorFromName);
      }
   }
}