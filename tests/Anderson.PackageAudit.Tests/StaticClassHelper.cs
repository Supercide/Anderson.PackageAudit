using System;
using System.Reflection;

namespace Anderson.PackageAudit.Tests
{
    public class StaticClassHelper
    {
        public static void Reset(Type type)
        {
            ConstructorInfo constructorInfo = type.TypeInitializer;
            object[] parameters = new object[0];
            constructorInfo.Invoke(null, parameters);
        }
    }
}