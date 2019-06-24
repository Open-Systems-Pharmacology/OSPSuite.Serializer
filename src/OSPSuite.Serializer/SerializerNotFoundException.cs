using System;

namespace OSPSuite.Serializer
{
   public class SerializerNotFoundException : Exception
   {
      private static string errorMessage(string objectName) => $"Unable to find a serializer for '{objectName}'";

      public SerializerNotFoundException(Type typeOfObject) : this(typeOfObject.ToString())
      {
      }

      public SerializerNotFoundException(string objectName) : base(errorMessage(objectName))
      {
      }

      public SerializerNotFoundException(Type typeOfObject, Type typeOfContainerObject)
         : base($"{errorMessage(typeOfObject.ToString())} in {typeOfContainerObject}")
      {
      }
   }
}