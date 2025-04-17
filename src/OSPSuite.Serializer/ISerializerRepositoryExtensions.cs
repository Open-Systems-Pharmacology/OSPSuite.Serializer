using System;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Serializer
{
   public static class XmlSerializerRepositoryExtensions
   {
      public static void AddSerializers<TContext>
         (this IXmlSerializerRepository<TContext> serializerRepository, Action<IAssemblySerializerScanner<TContext>> scan)
      {
         var assemblySerializerScanner = new AssemblySerializerScanner<TContext>(serializerRepository);
         scan(assemblySerializerScanner);
         assemblySerializerScanner.Scan();
      }
   }
}