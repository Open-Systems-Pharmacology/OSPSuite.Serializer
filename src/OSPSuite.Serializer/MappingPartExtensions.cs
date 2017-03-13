namespace OSPSuite.Serializer
{
   public static class MappingPartExtensions
   {
      public static T WithMappingName<T>(this T mappingPart, string mappingName) where T : IMappingPart
      {
         mappingPart.MappingName = mappingName;
         return mappingPart;
      }

      public static IEnumerableMappingPart<TObject, TProperty, TContext> WithChildMappingName<TObject, TProperty, TContext>(this IEnumerableMappingPart<TObject, TProperty, TContext> enumerableMappingPart, string childMappingName)
      {
         enumerableMappingPart.ChildMappingName = childMappingName;
         return enumerableMappingPart;
      }
   }
}