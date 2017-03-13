using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Serializer
{
   public static class SerializerExtensions
   {
      public static TSerializer WithRepositories<TSerializer, TContext>(this TSerializer serializer, IXmlSerializerRepository<TContext> serializerRepository,
         IAttributeMapperRepository<TContext> attributeMapperRepository) where TSerializer : IXmlSerializer<TContext>
      {
         serializer.SetRepositories(serializerRepository, attributeMapperRepository);
         return serializer;
      }
   }
}