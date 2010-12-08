using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace AppVisum.Sys.Utilities
{
    public class ObjectMerger : List<object>
    {
        private static volatile bool built;
        private static volatile int count = 0;
        private static AssemblyBuilder assemblyBuilder;
        private static ModuleBuilder moduleBuilder;
        private static readonly object buildLock = new object();

        private struct PropAndObj
        {
            public PropertyInfo PropertyInfo { get; set; }
            public Object Object { get; set; }
        }

        public object Merge()
        {
            Dictionary<String, PropAndObj> props = new Dictionary<String, PropAndObj>();
            foreach (var obj in this)
            {
                if (obj == null)
                    throw new Exception("Can not merge with null");

                Type t = obj.GetType();
                foreach (var prop in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    props[prop.Name] = new PropAndObj
                        {
                            PropertyInfo = prop,
                            Object = obj
                        };
            }

            lock (buildLock)
            {
                if (!built)
                {
                    assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                        new AssemblyName("AppVisum.Sys.Utilities.MergedTypes"),
                        AssemblyBuilderAccess.Run);

                    string assemblyName = assemblyBuilder.GetName().Name;

                    moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName);
                }
                built = true;
            }

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                String.Format("merged_type_{0}", count++),
                TypeAttributes.Class | TypeAttributes.Public,
                typeof(Object),
                new Type[] { });

            foreach (var p in props)
            {
                var fb = typeBuilder.DefineField("_" + p.Key, p.Value.PropertyInfo.PropertyType, FieldAttributes.Private);
                var pr = typeBuilder.DefineProperty(p.Key, PropertyAttributes.None, CallingConventions.HasThis, p.Value.PropertyInfo.PropertyType, null);
                var getMethod = typeBuilder.DefineMethod("get_" + p.Key, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, p.Value.PropertyInfo.PropertyType, Type.EmptyTypes);
                var setMethod = typeBuilder.DefineMethod("set_" + p.Key, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { p.Value.PropertyInfo.PropertyType });

                var getIl = getMethod.GetILGenerator();
                getIl.Emit(OpCodes.Ldarg_0);
                getIl.Emit(OpCodes.Ldfld, fb);
                getIl.Emit(OpCodes.Ret);

                var setIl = setMethod.GetILGenerator();
                setIl.Emit(OpCodes.Ldarg_0);
                setIl.Emit(OpCodes.Ldarg_1);
                setIl.Emit(OpCodes.Stfld, fb);
                setIl.Emit(OpCodes.Ret);

                pr.SetGetMethod(getMethod);
                pr.SetSetMethod(setMethod);
            }

            Type retType = typeBuilder.CreateType();
            object ret = Activator.CreateInstance(retType);

            foreach(var p in props)
            {
                var prop = retType.GetProperty(p.Key);
                prop.GetSetMethod().Invoke(ret, new object[] { p.Value.PropertyInfo.GetGetMethod().Invoke(p.Value.Object, new object[] { }) });
            }

            return ret;
        }
    }
}
