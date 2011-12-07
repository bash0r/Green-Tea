using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GreenTea
{
    internal class WrapperFunc : GTFunction
    {
        internal MethodInfo Method;

        public WrapperFunc(MethodInfo mi) : base(null, null, mi.GetParameters().Select(pi => pi.Name))
        {
            this.Method = mi;
        }
    }

    public static partial class Functions
    {
        private static Dictionary<string, WrapperFunc> Funcs = new Dictionary<string, WrapperFunc>();

        // example
        [Name("string")]
        public static Value stringify(Value s)
        {
            return new GTString(s.ToString());
        }

        static Functions()
        {
            foreach (var m in typeof(Functions).GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                var a = m.GetCustomAttributes(typeof(NameAttribute), false);
                string s;

                if (a.Length > 0)
                    s = ((NameAttribute)a[0]).Name;
                else
                    s = m.Name;

                Funcs.Add(s, new WrapperFunc(m));
            }
        }

        internal static void AddTo(Scope scope)
        {
            foreach (var kvp in Funcs)
                scope.Add(kvp.Key, kvp.Value, ScopeMode.Public);
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class NameAttribute : Attribute
    {
        public string Name { get; private set; }

        public NameAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}