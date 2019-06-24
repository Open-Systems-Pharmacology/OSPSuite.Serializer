using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Serializer.Xml
{
   public interface IXmlSerializerRepository<TContext>
   {
      /// <summary>
      /// Perform current mapping and throws an exception if any registered serializer is not valid
      /// </summary>
      void PerformMapping();

      /// <summary>
      /// Remove all avalaible serializer from the repository
      /// </summary>
      void Clear();

      /// <summary>
      ///    Retrieve a serializer by key
      /// </summary>
      /// <param name="serializerKey">Key with which a serializer was registered</param>
      IXmlSerializer<TContext> this[string serializerKey] { get; }

      /// <summary>
      ///    Register one serializer into the repository with its name als key
      ///    Note: Adding a serializer should only be called BEFORE calling the PerformMapping step.
      /// </summary>
      void AddSerializer(IXmlSerializer<TContext> serializer);

      /// <summary>
      ///    Register one serializer into the repository with the given key
      ///    Note: Adding a serializer should only be called BEFORE calling the PerformMapping step.
      /// </summary>
      /// <param name="key">key with which the serializer will be registered</param>
      /// <param name="serializer">serializer to register</param>
      void AddSerializer(string key, IXmlSerializer<TContext> serializer);

      /// <summary>
      ///    Remove the serializer registered with the given key.
      ///    Note: Removing serializer should only be called BEFORE calling the PerformMapping step.
      /// </summary>
      /// <param name="key">key of serializer</param>
      /// <exception cref="KeyNotFoundException"> is thrown if no serializer was registered with the given key</exception>
      void RemoveSerializer(string key);

      /// <summary>
      ///    Retrieve a serializer by key
      /// </summary>
      /// <param name="serializerKey">Key with which a serializer was registered</param>
      IXmlSerializer<TContext> SerializerByKey(string serializerKey);

      /// <summary>
      ///    Retrieve a serializer for the given type
      /// </summary>
      /// <typeparam name="TObjectToSerialize">type for which a serializer is requested</typeparam>
      /// <returns>A serializer for the given type if one was registered.</returns>
      /// <exception cref="SerializerNotFoundException">is thrown if no serializer is found for the given type</exception>
      IXmlSerializer<TContext> SerializerFor<TObjectToSerialize>();

      /// <summary>
      ///    Retrieve a serializer for the given type
      /// </summary>
      /// <typeparam name="TObjectToSerialize">type for which a serializer is requested</typeparam>
      /// <returns>A serializer for the given type if one was registered, otherwise null</returns>
      IXmlSerializer<TContext> SerializerOrDefaultFor<TObjectToSerialize>();

      /// <summary>
      ///    Retrieve a serializer for the given type
      /// </summary>
      /// <param name="typeOfObjectToSerialize">type for which a serializer is requested</param>
      /// <returns>A serializer for the given type if one was registered</returns>
      /// <exception cref="SerializerNotFoundException">is thrown if no serializer is found for the given type</exception>
      IXmlSerializer<TContext> SerializerFor(Type typeOfObjectToSerialize);

      /// <summary>
      ///    Retrieve a serializer for the given type
      /// </summary>
      /// <param name="typeOfObjectToSerialize">type for which a serializer is requested</param>
      /// <returns>A serializer for the given type if one was registered, otherwise null</returns>
      IXmlSerializer<TContext> SerializerOrDefaultFor(Type typeOfObjectToSerialize);

      /// <summary>
      ///    Retrieve a serializer for the given instance
      /// </summary>
      /// <typeparam name="TObjectToSerialize">type for which a serializer is requested</typeparam>
      /// <param name="instanceThatNeedToBeSerialized">instance for which a serializer is requested</param>
      /// <returns>A serializer for the given type if one was registered, otherwise null</returns>
      IXmlSerializer<TContext> SerializerFor<TObjectToSerialize>(TObjectToSerialize instanceThatNeedToBeSerialized);

      /// <summary>
      ///    Return the list of all available serializers for a given type
      /// </summary>
      /// <param name="typeOfObjectToSerialize">type for which all registeres serializer are requested</param>
      /// <returns>the list of all availale serializers for a given type</returns>
      IReadOnlyList<IXmlSerializer<TContext>> AllPossibleSerializerFor(Type typeOfObjectToSerialize);

      XNamespace Namespace { get; set; }
      bool NeedsDeserialization { get; set; }

      /// <summary>
      ///    if set to true, the node for an empty enumeration will be generated. Otherwise node will be ignored
      ///    Default is false;
      /// </summary>
      bool CreateNodeForEmptyEnumeration { get; set; }

      /// <summary>
      ///    Retrieve a serializer for the given element
      /// </summary>
      /// <param name="element">XElement for which a serializer is requested</param>
      /// <returns>A serializer for the given element if one was registered.</returns>
      /// <exception cref="SerializerNotFoundException">is thrown if no serializer is found for the given element</exception>
      IXmlSerializer<TContext> SerializerFor(XElement element);

      /// <summary>
      ///    Creates and returns an element with the given name
      /// </summary>
      XElement CreateElement(string elementName);

      /// <summary>
      ///    Renames the given element
      /// </summary>
      string ElementNameFor(string elementName);
   }

   public class XmlSerializerRepository<TContext> : IXmlSerializerRepository<TContext>
   {
      public XNamespace Namespace { get; set; }
      public bool NeedsDeserialization { get; set; }
      public bool CreateNodeForEmptyEnumeration { get; set; }

      public XmlSerializerRepository()
      {
         _typeSimplifier = new TypeSimplifier();
         Namespace = string.Empty;
         NeedsDeserialization = true;
         CreateNodeForEmptyEnumeration = false;
      }

      private readonly ICache<string, IXmlSerializer<TContext>> _allSerializer = new Cache<string, IXmlSerializer<TContext>>();
      private readonly ConcurrentDictionary<Type, IXmlSerializer<TContext>> _typeSerializerCache = new ConcurrentDictionary<Type, IXmlSerializer<TContext>>();

      private readonly ITypeSimplifier _typeSimplifier;
      private bool _mappingPerformed;

      public virtual void AddSerializer(IXmlSerializer<TContext> serializer)
      {
         AddSerializer(serializer.Name, serializer);
      }

      public void AddSerializer(string key, IXmlSerializer<TContext> serializer)
      {
         if (_allSerializer.Contains(key))
            throw new ArgumentException($"A serializer with key '{key}' for type '{serializer.ObjectType}' is already registered in the repository '{GetType()}'.");

         if (_mappingPerformed)
            throw new ArgumentException($"Mapping was already performed (e.g. PerformMapping was called). Cannot add a serialier with key '{key}' for type '{serializer.ObjectType}' in repository '{GetType()}'.");

         _allSerializer.Add(key, serializer);
      }

      public void RemoveSerializer(string key)
      {
         if (_mappingPerformed)
            throw new ArgumentException($"Mapping was already performed (e.g. PerformMapping was called). Cannot remove a serialier with key '{key}' in repository '{GetType()}'.");
         if (!_allSerializer.Contains(key))
            throw new KeyNotFoundException($"Serialier with key '{key}' was not found in repository '{GetType()}'.");

         _allSerializer.Remove(key);
      }

      public IXmlSerializer<TContext> this[string serializerKey] => SerializerByKey(serializerKey);

      public IXmlSerializer<TContext> SerializerByKey(string serializerKey)
      {
         return _allSerializer.Contains(serializerKey) ? _allSerializer[serializerKey] : null;
      }

      public virtual IXmlSerializer<TContext> SerializerFor<TObjectToSerialize>()
      {
         return SerializerFor(typeof (TObjectToSerialize));
      }

      public IXmlSerializer<TContext> SerializerOrDefaultFor<TObjectToSerialize>()
      {
         return SerializerOrDefaultFor(typeof (TObjectToSerialize));
      }

      public IXmlSerializer<TContext> SerializerFor(Type typeOfObjectToSerialize)
      {
         var serializer = SerializerOrDefaultFor(typeOfObjectToSerialize);
         if (serializer != null)
            return serializer;

         throw new SerializerNotFoundException(typeOfObjectToSerialize);
      }

      public IXmlSerializer<TContext> SerializerOrDefaultFor(Type typeOfObjectToSerialize)
      {
         IXmlSerializer<TContext> serializer;
         if (_typeSerializerCache.TryGetValue(typeOfObjectToSerialize, out serializer))
            return serializer;

         serializer = retrieveSerializerForType(typeOfObjectToSerialize);

         return _typeSerializerCache.GetOrAdd(typeOfObjectToSerialize, serializer);
      }

      public IXmlSerializer<TContext> SerializerFor<TObjectToSerialize>(TObjectToSerialize instanceThatNeedToBeSerialized)
      {
         return SerializerFor(instanceThatNeedToBeSerialized.GetType());
      }

      public IReadOnlyList<IXmlSerializer<TContext>> AllPossibleSerializerFor(Type typeOfObjectToSerialize)
      {
         var query = from serializer in _allSerializer
            where serializer.ObjectType.IsAnImplementationOf(typeOfObjectToSerialize)
            select serializer;

         return query.ToList();
      }

      public virtual void PerformMapping()
      {
         //first perform the mapping
         _allSerializer.Each(serializer => serializer.PerformMapping());

         //then create the mapper
         _allSerializer.Each(serializer => serializer.Build());

         _mappingPerformed = true;
      }

      public void Clear()
      {
         _allSerializer.Clear();
      }

      private IXmlSerializer<TContext> retrieveSerializerForType(Type typeOfObjectToSerialize)
      {
         var matchingSerializers = _allSerializer.Where(item => item.ObjectType == typeOfObjectToSerialize).ToList();
         if (matchingSerializers.Count == 1)
            return matchingSerializers[0];

         if (matchingSerializers.Count > 1)
            throw new AmbiguousSerializerException(matchingSerializers, typeOfObjectToSerialize);

         //do we have a serializer that matches the inhouse conventions
         matchingSerializers = _allSerializer.Where(item => $"I{item.ObjectType.Name}" == typeOfObjectToSerialize.Name).ToList();
         if (matchingSerializers.Count == 1)
            return matchingSerializers[0];

         var query = from serializer in _allSerializer
            where typeOfObjectToSerialize.IsAnImplementationOf(serializer.ObjectType)
            select serializer;

         var allPossibleSerializers = query.ToList();

         //no matching signatures, return 
         if (allPossibleSerializers.Count == 0)
            return null;

         //one matching signature that the one
         if (allPossibleSerializers.Count == 1)
            return allPossibleSerializers[0];

         //more than one? try to reduce the object type hierarchy
         var simplifiedSerializer = simplify(allPossibleSerializers).ToList();
         if (simplifiedSerializer.Count == 1)
            return simplifiedSerializer[0];

         //at least two implementations. Serialization call is ambiguous and cannot be resolved at run time.
         throw new AmbiguousSerializerException(allPossibleSerializers, typeOfObjectToSerialize);
      }

      private IEnumerable<IXmlSerializer<TContext>> simplify(IReadOnlyList<IXmlSerializer<TContext>> allPossibleSerializers)
      {
         var simplifiedTypes = _typeSimplifier.Simplify(allPossibleSerializers.Select(s => s.ObjectType));

         return from s in allPossibleSerializers
            where simplifiedTypes.Contains(s.ObjectType)
            select s;
      }

      public IXmlSerializer<TContext> SerializerFor(XElement element)
      {
         var elementName = element.Name.LocalName;
         var serializer = SerializerByKey(elementName);
         if (serializer != null) return serializer;
         throw new SerializerNotFoundException(elementName);
      }

      public XElement CreateElement(string elementName)
      {
         return new XElement(Namespace + elementName);
      }

      public string ElementNameFor(string elementName)
      {
         var xname = Namespace + elementName;
         return xname.ToString();
      }

      public XElement RenameElement(XElement element, string newName)
      {
         return new XElement(Namespace + newName, element.Attributes(), element.Nodes());
      }
   }
}