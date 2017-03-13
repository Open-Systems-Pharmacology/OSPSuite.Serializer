using System.Linq;
using System.Xml.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Serializer.Xml
{
   /// <summary>
   /// Add a default implementation for a cache serializer of TKey and TValue
   /// The TKey and TValue should not be objects that would be otherwise serialized as attributes 
   /// The implementation bypasses the default add method if defined. Create method should be overriden
   /// if a specific add method was defined
   /// </summary>
   public class XmlCacheSerializer<TKey, TValue, TContext> : XmlSerializer<ICache<TKey, TValue>, TContext>
   {
      private const string _keyValueDefaultName = "KeyValue";

      public XmlCacheSerializer() : this(string.Format("Cache{0}{1}", typeof (TKey).Name, typeof (TValue).Name))
      {
      }

      public XmlCacheSerializer(string name) : base(name)
      {
         KeyValueNode = _keyValueDefaultName;
      }

      protected virtual string KeyValueNode { get; set; }

      public override void PerformMapping()
      {
         /*nothing to do here*/
      }

      protected override void TypedDeserialize(ICache<TKey, TValue> cache, XElement cacheElement, TContext context)
      {
         foreach (XElement keyValueElement in cacheElement.Elements(KeyValueNode))
         {
            //not right format
            if (keyValueElement.Elements().Count() < 2) continue;
            var keyNode = keyValueElement.Elements().ElementAt(0);
            var valueNode = keyValueElement.Elements().ElementAt(1);

            var key = SerializerRepository.SerializerFor(keyNode).Deserialize<TKey>(keyNode,context);
            var value = SerializerRepository.SerializerFor(valueNode).Deserialize<TValue>(valueNode, context);
            cache.Add(key, value);
         }
      }

      protected override XElement TypedSerialize(ICache<TKey, TValue> cache, TContext context)
      {
         var cacheElement = SerializerRepository.CreateElement(ElementName);

         foreach (var keyValue in cache.KeyValues)
         {
            var keyValueElement = SerializerRepository.CreateElement(_keyValueDefaultName);
            keyValueElement.Add(SerializerRepository.SerializerFor(keyValue.Key).Serialize(keyValue.Key, context));
            keyValueElement.Add(SerializerRepository.SerializerFor(keyValue.Value).Serialize(keyValue.Value, context));
            cacheElement.Add(keyValueElement);
         }
         return cacheElement;
      }

      public override ICache<TKey, TValue> CreateObject(XElement element, TContext context)
      {
         return new Cache<TKey, TValue>();
      }
   }
}