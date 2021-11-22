using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests
{
    public static class PropertyExtensions
    {
        public static void SetProperty<TProperty, T>(this T instance,
            Expression<Func<T, TProperty>> propertyPicker,
            TProperty value) where T : class
        {
            var exp = propertyPicker.Body as MemberExpression;
            if (exp == null)
            {
                return;
            }
            
            var propertyInfo = (PropertyInfo)(exp.Member);
            propertyInfo.SetValue(instance, value);
        }
    }
}
