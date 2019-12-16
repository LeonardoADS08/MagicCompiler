using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MagicCompiler.Tools
{
    public static class AssemblyExtension
    {
        public static object CreateInstance(this Assembly assembly, string typeName)
        {
            var type = assembly.GetType(typeName);
            var instance = assembly.CreateInstance(typeName);
            return instance;
        }

        public static T CreateInstance<T>(this Assembly assembly, string typeName)
        {
            var type = assembly.GetType(typeName);
            var instance = assembly.CreateInstance(typeName);
            return (T)instance;
        }

        public static T CallMethod<T>(this object instance, string typeName, string methodName, object[] parameters)
        {
            var type = instance.GetType();
            var meth = type.GetMember(methodName).First() as MethodInfo;
            return (T)meth.Invoke(instance, parameters);
        }

        public static T GetProperty<T>(this object instance, string propertyName)
        {
            var type = instance.GetType();
            var property = type.GetProperty(propertyName);
            return (T) property.GetValue(instance);
        }

        public static void SetProperty<T>(this object instance, string propertyName, T newValue)
        {
            var type = instance.GetType();
            var property = type.GetProperty(propertyName);
            property.SetValue(instance, newValue);
        }

        public static T GetField<T>(this object instance, string memberName)
        {
            var type = instance.GetType();
            var field = type.GetField(memberName);
            return (T)field.GetValue(instance);
        }

        public static void SetField<T>(this object instance, string memberName, T newValue)
        {
            var type = instance.GetType();
            var field = type.GetField(memberName);
            field.SetValue(instance, newValue);
        }
    }
}
