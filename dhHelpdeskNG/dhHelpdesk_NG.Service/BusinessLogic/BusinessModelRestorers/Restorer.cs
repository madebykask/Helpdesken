namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public abstract class Restorer
    {
        protected void RestoreFieldIfNeeded<TValue>(
            object sourceObject,
            Expression<Func<TValue>> property,
            object existingValue,
            bool show)
        {
            if (show)
            {
                return;
            }

            var expressionMember = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)expressionMember.Member;
            propertyInfo.SetValue(sourceObject, existingValue, null);
        }
    }
}