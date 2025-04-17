using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Serializer.Reflection
{
   public interface IMemberAccessorFactory
   {
      IMemberAccessor CreateFor<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression);
      IMemberAccessor CreateFor<TObject, TProperty>(string memberName);
   }

   internal class MemberAccessorFactory : IMemberAccessorFactory
   {
      public IMemberAccessor CreateFor<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
      {
         var memberExpression = new ExpressionInspector<TObject>().GetMemberExpression(expression);
         var member = memberExpression.Member;

         //We have a field. Return a simple field accessor
         if (member is FieldInfo info)
            return new FieldAccessor(info);

         //We have a property. If the property is read write, we can simply return a property accessor
         var propertyInfo = member as PropertyInfo;
         if (propertyInfo == null)
            throw new MemberAccessException(member);

         if (propertyInfo.CanWrite)
            return new ReadWritePropertyAccessor(propertyInfo);

         //we have a readonly property. Try to find a backing field corresponding to the property
         var fieldInfos = typeof(TObject).AllFields().Where(mi => privateMemberMatchesNamingConventions(mi, member.Name)).ToList();

         //we found one, return the property accessor
         if (fieldInfos.Count == 1)
            return new ReadOnlyPropertyWithPrivateFieldAccessor(propertyInfo, fieldInfos[0]);

         //last try: Find a property that might be read only and return it
         var propertyInfos = typeof(TObject).AllProperties().Where(prop => prop.Name == member.Name).ToList();
         if (propertyInfos.Count == 1)
            propertyInfo = propertyInfos[0];

         if (propertyInfo.CanWrite)
            return new ReadWritePropertyAccessor(propertyInfo);

         return new ReadOnlyPropertyAccessor(propertyInfo);
      }

      public IMemberAccessor CreateFor<TObject, TProperty>(string memberName)
      {
         var allFields = typeof(TObject).AllFields().Where(type => type.Name == memberName)
            .Where(type => type.FieldType == typeof(TProperty)).ToList();

         if (allFields.Count != 1)
            throw new MemberAccessException(typeof(TObject), typeof(TProperty), memberName);

         var field = allFields[0];
         if (field.IsInitOnly)
            throw new MemberAccessException(typeof(TObject), typeof(TProperty), memberName);

         return new FieldAccessor(field);
      }

      private bool privateMemberMatchesNamingConventions(MemberInfo memberInfo, string propertyName) =>
         MemberNamingConventions.AllConventions.Any(x => x.Matches(memberInfo.Name, propertyName));
   }
}