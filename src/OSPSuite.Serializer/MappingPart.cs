namespace OSPSuite.Serializer
{
   public interface IMappingPart
   {
      string MappingName { get; set; }
      string ExpressionName { get; }
      IPropertyMap PropertyMap { get; }
   }

   internal abstract class MappingPart : IMappingPart
   {
      public string MappingName { get; set; }
      public IPropertyMap PropertyMap { get; }
      public string ExpressionName => PropertyMap.Name;

      protected MappingPart(IPropertyMap propertyMap)
      {
         PropertyMap = propertyMap;
      }
   }
}