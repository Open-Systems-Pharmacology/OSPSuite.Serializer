using System.Xml.Linq;

namespace OSPSuite.Serializer.Xml
{
   public interface IXmlMapper<in TObject, in TContext>
   {
      /// <summary>
      ///    Name of element or attribute used in the xml node
      /// </summary>
      string MappingName { get; set; }

      XObject Serialize(TObject objectToSerialize, TContext context);
      void Deserialize(TObject objectToDeserialize, XElement element, TContext context);
   }
}