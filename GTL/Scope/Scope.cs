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

        public Scope(Scope parent) : this(parent, parent == null ? null : parent.Namespace) { }
        public Scope() : this(null, null) { }

        public Scope Close()
        {
            // create a new child closure
            Scope s = new Scope(null, Namespace);

            // Duplicate all variables
            foreach (var v in Variables)
                s.Add(v.Key, v.Value);

            if (Parent != null)
                s.Parent = Parent.Close(); // recurse upwards

            return s;
        }

        public void Add(string name, Value val, ScopeMode mode = ScopeMode.Local)
        {
            switch (mode)
            {
                case ScopeMode.Local:
                    Variables.Add(name, val);
                    break;

                case ScopeMode.Export:
                    Namespace.Root.Add(name, val);
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

            // reached root, try static
            if (Namespace != null)
                return Namespace.Static.TryFind(name, out ret, newval);

            ret = GTVoid.Void;
            return false;
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
