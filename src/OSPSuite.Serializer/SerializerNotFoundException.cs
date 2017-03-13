using System;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer
{
   public class SerializerNotFoundException : Exception
   {
      private const string _errorMessage = "Unable to find a serializer for '{0}'";

      public SerializerNotFoundException(Type typeOfObject) : this(typeOfObject.ToString())
      {
      }

      public SerializerNotFoundException(string objectName) : base(_errorMessage.FormatWith(objectName))
      {
      }

      public SerializerNotFoundException(Type typeOfObject, Type typeOfContainerObject)
         : base(string.Format("{0} in {1}", _errorMessage.FormatWith(typeOfObject), typeOfContainerObject))
      {
      }
   }
}