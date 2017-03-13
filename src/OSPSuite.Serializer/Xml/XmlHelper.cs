using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace OSPSuite.Serializer.Xml
{
   public static class XmlHelper
   {
      public static XElement ElementFromString(string serializationString)
      {
         var document = documentFromString(serializationString);
         return document.Root;
      }

      public static XElement ElementFromBytes(byte[] serializationByte)
      {
         try
         {
            using (var stream = new MemoryStream(serializationByte))
            using (var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
            {
               return XElement.Load(reader);
            }
         }
         catch
         {
            return ElementFromString(Encoding.UTF8.GetString(serializationByte));
         }
      }

      private static XDocument documentFromString(string serializationString)
      {
         // Encode the XML string in a UTF-8 byte array
         byte[] encodedString = Encoding.UTF8.GetBytes(serializationString);

         // Put the byte array into a stream and rewind it to the beginning
         using (var ms = new MemoryStream(encodedString))
         using (var xmlReader = new XmlTextReader(ms))
         {
            return XDocument.Load(xmlReader);
         }
      }

      /// <summary>
      ///    Return the content of the file whose path was specified with the given parameter
      /// </summary>
      public static Func<string, string> XmlContentFromFile = xmlContentFromFile;

      private static string xmlContentFromFile(string fileName)
      {
         using (var fileReader = new StreamReader(fileName, Encoding.UTF8))
         {
            return fileReader.ReadToEnd();
         }
      }

      /// <summary>
      ///    returns the string representing the xml element and remove the formatting
      /// </summary>
      public static string XmlContentToString(XElement element)
      {
         return element.ToString(SaveOptions.DisableFormatting);
      }

      public static byte[] XmlContentToByte(XElement element)
      {
         using (var stream = new MemoryStream())
         {
            using (var textWriter = new XmlTextWriter(stream, Encoding.UTF8))
            {
               textWriter.Formatting = Formatting.None;
               textWriter.Indentation = 0;
               element.Save(textWriter);
            }
            return stream.ToArray();
         }
      }

      /// <summary>
      ///    Save the content of the first parameter to the file whose path was specified with the second parameter
      /// </summary>
      public static Action<string, string> SaveXmlContentToFile = saveXmlContentToFile;

      private static void saveXmlContentToFile(string xmlContent, string fileName)
      {
         var doc = documentFromString(xmlContent);
         saveXmlObjectToFile(fileName, doc.Save);
      }

      public static Action<XElement, string> SaveXmlElementToFile = saveXmlElementToFile;

      private static void saveXmlElementToFile(XElement element, string fileName)
      {
         saveXmlObjectToFile(fileName, element.Save);
      }

      private static void saveXmlObjectToFile(string fileName, Action<XmlTextWriter> saveAction)
      {
         //---- DO NOT write Unicode byte order mark into the XML file
         //     (otherwise it's not readable in japanese/chinese/... OS)
         //     (s. http://stackoverflow.com/questions/4942825/xdocument-saving-xml-to-file-without-bom)
         using (var writer = new XmlTextWriter(fileName, new UTF8Encoding(false)))
         {
            writer.Formatting = Formatting.Indented;
            saveAction(writer);
         }
      }

      /// <summary>
      ///    Only use for tests
      /// </summary>
      public static void Reset()
      {
         SaveXmlContentToFile = saveXmlContentToFile;
         SaveXmlElementToFile = saveXmlElementToFile;
         XmlContentFromFile = xmlContentFromFile;
      }
   }
}