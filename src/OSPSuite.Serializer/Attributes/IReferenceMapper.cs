namespace OSPSuite.Serializer.Attributes
{
   /// <summary>
   /// Allows to save reference to objects instead of nodes. The way the references
   /// are defined is left to the implementation
   /// </summary>
   public interface IReferenceMapper<in TSerializationContext>
   {
      /// <summary>
      /// Returns the reference for the object to convert
      /// </summary>
      /// <param name="valueToConvert">value for which a reference should be returned</param>
      /// <param name="serializationContext">Serialization context</param>
      /// <returns>the reference to the object</returns>
      string ReferenceFor(object valueToConvert, TSerializationContext serializationContext);

      /// <summary>
      /// Given the object to deserialize and the reference Name, the implementation should map the refernce value to the reference name
      /// </summary>
      /// <param name="objectToDeserialize">object to deserialize</param>
      /// <param name="propertyMap">Property of the object being actually mapped </param>
      /// <param name="referenceValue">reference of value to be set</param>
      /// <param name="serializationContext">Serialization context</param>
      void ResolveReference(object objectToDeserialize, IPropertyMap propertyMap, string referenceValue, TSerializationContext serializationContext);
   }
}