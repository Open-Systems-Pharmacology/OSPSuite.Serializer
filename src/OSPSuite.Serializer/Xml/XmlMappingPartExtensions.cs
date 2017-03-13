namespace OSPSuite.Serializer.Xml
{
   public static class XmlMappingPartExtensions
   {
      public static T AsNode<T>(this T xmlMappingPart) where T : IXmlMappingPart
      {
         xmlMappingPart.IsAttribute = false;
         xmlMappingPart.IsNode = true;
         return xmlMappingPart;
      }

      public static T AsAttribute<T>(this T xmlMappingPart) where T : IXmlMappingPart
      {
         xmlMappingPart.IsNode = false;
         xmlMappingPart.IsAttribute = true;
         return xmlMappingPart;
      }
   }
}