using System;
using System.Collections.Generic;
using System.Reflection;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Serializer
{
   public interface IAssemblySerializerScanner<TContext>
   {
      void Implementing<T>() where T : class, IXmlSerializer;
      void InAssemblyContainingType<T>();
      void UsingAttributeRepository(IAttributeMapperRepository<TContext> attributeMapperRepository);
      void Scan();
   }

   public class AssemblySerializerScanner<TContext> : IAssemblySerializerScanner<TContext>
   {
      private readonly IXmlSerializerRepository<TContext> _serializerRepository;
      private Assembly _assembly;
      private IAttributeMapperRepository<TContext> _attributeMapperRepository;
      private Type _serializerType;

      public AssemblySerializerScanner(IXmlSerializerRepository<TContext> serializerRepository)
      {
         _serializerRepository = serializerRepository;
      }

      public void Implementing<T>() where T : class, IXmlSerializer
      {
         _serializerType = typeof (T);
      }

      public void InAssemblyContainingType<T>()
      {
         _assembly = typeof (T).Assembly;
      }

      public void UsingAttributeRepository(IAttributeMapperRepository<TContext> attributeMapperRepository)
      {
         _attributeMapperRepository = attributeMapperRepository;
      }

      public void Scan()
      {
         verifyConfiguration();
         IEnumerable<Type> allSerializerTypes = _assembly.GetAllConcreteTypesImplementing(_serializerType);

         allSerializerTypes.Each(addSerializerToRepository);
      }

      private void addSerializerToRepository(Type serializerType)
      {
         var serializer = serializerType.CreateInstance<IXmlSerializer<TContext>>();
         serializer.SetRepositories(_serializerRepository, _attributeMapperRepository);
         _serializerRepository.AddSerializer(serializer);
      }

      private void verifyConfiguration()
      {
         if (_serializerType == null)
            throw new ArgumentException("Interface implemented by serializer was not defined for the scanner. Please use x=>x.Implementing<T>");

         if (_assembly == null)
            throw new ArgumentException("Assembly to scan for serializers was not defined for the scanner. Please use x=>x.InAssemblyContainingType<T>");

         if (_attributeMapperRepository == null)
            throw new ArgumentException("Attribute mapper repository was not defined for the scanner. Please use x=>x.UsingAttributeRepository");
      }
   }
}