using System.Reflection;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Serializer.Reflection
{
   internal class FieldAccessor : MemberAccessorBase
   {
      private readonly FieldInfo _fieldInfo;
      private readonly GetHandler _getHandler;
      private readonly bool _isInternal;
      private readonly SetHandler _setHandler;

      public FieldAccessor(FieldInfo fieldInfo) : base(fieldInfo.Name, fieldInfo.FieldType)
      {
         _fieldInfo = fieldInfo;
         _getHandler = DelegateFactory.CreateGet(fieldInfo);
         _setHandler = DelegateFactory.CreateSet(fieldInfo);

         _isInternal = _fieldInfo.IsPrivate || _fieldInfo.IsFamily;
      }

      public override object GetValue(object source)
      {
         return _getHandler(source);
      }

      public override void SetValue(object destination, object value)
      {
         if (_isInternal)
            _fieldInfo.SetValue(destination, value);
         else
            _setHandler(destination, value);
      }
   }
}