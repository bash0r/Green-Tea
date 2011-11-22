using System.Collections.Generic;

namespace GreenTea
{
    public sealed class Scope
    {
        private Dictionary<string, Value> Variables { get; set; }
        public Scope Parent { get; private set; }
        public Module Namespace { get; private set; }

        public Scope(Scope parent, Module mod)
        {
            this.Parent = parent;
            this.Namespace = mod;

            this.Variables = new Dictionary<string, Value>();
        }

        public void Add(string name, Value val, ScopeMode mode = ScopeMode.Local)
        {
            switch (mode)
            {
                case ScopeMode.Local:
                    Variables.Add(name, val);
                    break;

                case ScopeMode.Export:
                    Namespace.Export.Add(name, val);
                    break;

                case ScopeMode.Static:
                    Namespace.Static.Add(name, val);
                    break;
            }
        }

        public bool TryFind(string name, out Value ret, Value newval = null)
        {
            // check for local copy
            if (Variables.ContainsKey(name))
            {
                if (newval != null)
                    Variables[name] = newval;

                ret = Variables[name];
                return true;
            }

            // otherwise recurse upwards
            if (Parent != null)
                return Parent.TryFind(name, out ret, newval);

            // recursion reached root
            if (Namespace.Export.TryFind(name, out ret, newval))
                return true;

            return Namespace.Static.TryFind(name, out ret, newval);
        }

        public Value Find(string name, Value newval = null)
        {
            Value ret;

            if (!TryFind(name, out ret, newval))
                throw new KeyNotFoundException("Unknown variable: " + name);

            return ret;
        }
    }

    public enum ScopeMode
    {
        Local, Export, Static
    }
}
