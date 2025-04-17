using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace OSPSuite.Serializer.Xml.Extensions
{
   public static class XmlLinqExtensions
   {
      public static XElement AddElement(this XElement parentElement, XElement newElement)
      {
         parentElement.Add(newElement);
         return newElement;
      }

      public static XElement AddAttribute(this XElement element, string attributeName, string attributeValue)
      {
         var newAttribute = new XAttribute(attributeName, attributeValue ?? string.Empty);
         element.Add(newAttribute);
         return element;
      }

      public static XElement AddAttribute(this XElement element, string attributeName, int attributeValue) =>
         AddAttribute(element, attributeName, attributeValue.ToString(NumberFormatInfo.InvariantInfo));

      public static XElement AddAttribute(this XElement element, string attributeName, double attributeValue) =>
         AddAttribute(element, attributeName, attributeValue.ToString(NumberFormatInfo.InvariantInfo));

      public static XElement AddAttribute(this XElement element, string attributeName, float attributeValue) =>
         AddAttribute(element, attributeName, attributeValue.ToString(NumberFormatInfo.InvariantInfo));

      public static string GetAttribute(this XElement element, string attributeName)
      {
         var attribute = element.Attribute(attributeName);
         return attribute?.Value ?? string.Empty;
      }

      public static XElement GetFirstElementWithAttribute(this XElement element, string attributeName, string attributeValue)
      {
         var query = from childElement in element.Elements()
            where string.Equals(childElement.GetAttribute(SerializerConstants.DynamicName), attributeValue)
            select childElement;

         return query.FirstOrDefault();
      }
   }
}