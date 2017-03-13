using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Serializer.Xml.Extensions;

namespace OSPSuite.Serializer.Tests
{
   public abstract class concern_for_xml_linq_extensions : StaticContextSpecification
   {
      protected XElement _element;

      protected override void Context()
      {
         _element = new XElement("tralala");
      }
   }

   public class When_adding_an_attribute_to_a_given_element : concern_for_xml_linq_extensions
   {
      private XElement _result;

      protected override void Because()
      {
         _result = _element.AddAttribute("Toto", "value");
      }

      [Observation]
      public void should_add_the_attribute_with_the_given_name_and_value()
      {
         var attribute = _element.Attribute("Toto");
         attribute.ShouldNotBeNull();
         attribute.Value.ShouldBeEqualTo("value");
      }

      [Observation]
      public void should_return_the_element()
      {
         _result.ShouldBeEqualTo(_element);
      }
   }

   public class When_adding_an_element : concern_for_xml_linq_extensions
   {
      private XElement _result;

      protected override void Because()
      {
         _result = _element.AddElement(new XElement("toto"));
      }

      [Observation]
      public void should_add_the_element_with_the_given_name_as_sub_element_of_the_parent_element()
      {
         _result.Name.ToString().ShouldBeEqualTo("toto");
         _result.Parent.ShouldBeEqualTo(_element);
      }
   }
}