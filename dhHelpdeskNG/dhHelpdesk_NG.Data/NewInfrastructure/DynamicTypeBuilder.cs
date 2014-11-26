// Copyright 2013 Thomas Miliopoulos
// This file is a part of DynamicSelectExtensions and is licensed under the MS-PL
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html
// https://github.com/thiscode/DynamicSelectExtensions

namespace DH.Helpdesk.Dal.NewInfrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    // Thanks to Ethan J. Brown:
    // http://stackoverflow.com/questions/606104/how-to-create-linq-expression-tree-with-anonymous-type-in-it/723018#723018
    public static class DynamicTypeBuilder
    {
        private static readonly AssemblyName AssemblyName = new AssemblyName { Name = "DynamicLinqTypes" };

        private static readonly ModuleBuilder ModuleBuilder;

        private static readonly Dictionary<string, Tuple<string, Type>> BuiltTypes = new Dictionary<string, Tuple<string, Type>>();

        static DynamicTypeBuilder()
        {
            ModuleBuilder = Thread.GetDomain().DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(AssemblyName.Name);
        }

        public static Type GetDynamicType(Dictionary<string, Type> fields, Type basetype, Type[] interfaces)
        {
            if (null == fields)
            {
                throw new ArgumentNullException("fields");
            }

            if (0 == fields.Count)
            {
                throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");
            }

            try
            {
                Monitor.Enter(BuiltTypes);
                string typeKey = GetTypeKey(fields);

                if (BuiltTypes.ContainsKey(typeKey))
                {
                    return BuiltTypes[typeKey].Item2;
                }

                string typeName = "DynamicLinqType" + BuiltTypes.Count.ToString(CultureInfo.InvariantCulture);
                TypeBuilder typeBuilder = ModuleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable, null, Type.EmptyTypes);

                foreach (var field in fields)
                {
                    typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);
                }

                BuiltTypes[typeKey] = new Tuple<string, Type>(typeName, typeBuilder.CreateType());

                return BuiltTypes[typeKey].Item2;
            }
            finally
            {
                Monitor.Exit(BuiltTypes);
            }
        }

        private static string GetTypeKey(Dictionary<string, Type> fields)
        {
            return fields.OrderBy(v => v.Key).ThenBy(v => v.Value.Name).Aggregate(string.Empty, (current, field) => current + (field.Key + ";" + field.Value.Name + ";"));
        }
    }
}
