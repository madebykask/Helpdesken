﻿using System;
using System.Linq;

namespace DH.Helpdesk.TaskScheduler.Helpers
{
    public static class PropertyHelper
    {
        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            if (property == null)
                return null;

            return (T)property.GetCustomAttributes(attrType, false).First();
        }
    }
}
