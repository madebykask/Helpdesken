using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Infrastructure.Binders
{
    public class BindToListAttribute : Attribute
    {
    }

    public class CommaSeparatedListsBinder : DefaultModelBinder
    {
        private static readonly MethodInfo ToArrayMethod = typeof(Enumerable).GetMethod("ToArray");

        protected override object GetPropertyValue(ControllerContext controllerContext, 
                                                   ModelBindingContext bindingContext,
                                                   PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            if (propertyDescriptor.PropertyType.GetInterface(typeof(IEnumerable).Name) != null)
            {
                var actualValue = bindingContext.ValueProvider.GetValue(propertyDescriptor.Name);
                if (!string.IsNullOrWhiteSpace(actualValue?.AttemptedValue) && 
                    (propertyDescriptor.PropertyType.HasAttribute(typeof(BindToListAttribute)) ||
                     actualValue.AttemptedValue.Contains(",")))
                {
                    var valueType = propertyDescriptor.PropertyType.GetElementType() ??
                                    propertyDescriptor.PropertyType.GetGenericArguments().FirstOrDefault();

                    if (valueType != null && valueType.GetInterface(typeof(IConvertible).Name) != null)
                    {
                        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(valueType));
                        foreach (var splitValue in actualValue.AttemptedValue.Split(new[] { ',' }))
                        {
                            if (valueType.IsEnum)
                            {
                                try
                                {
                                    list.Add(Enum.Parse(valueType, splitValue));
                                }
                                catch { }
                            }
                            else
                            {
                                list.Add(Convert.ChangeType(splitValue, valueType));
                            }
                        }

                        if (propertyDescriptor.PropertyType.IsArray)
                        {
                            return ToArrayMethod.MakeGenericMethod(valueType).Invoke(this, new[] { list });
                        }

                        return list;
                    }
                }
            }

            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }
    }
}