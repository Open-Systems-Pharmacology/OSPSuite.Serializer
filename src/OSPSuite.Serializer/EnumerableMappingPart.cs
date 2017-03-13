using System;

namespace OSPSuite.Serializer
{
   public interface IEnumerableMappingPart<TObject, TProperty, TContext> : IMappingPart
   {
      Func<TObject, TContext, Action<TProperty>> AddFunction { set; }
      string ChildMappingName { get; set; }
   }
}