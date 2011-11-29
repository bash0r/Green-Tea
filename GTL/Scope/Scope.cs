using System.Collections.Generic;

namespace GreenTea
{
    public sealed class Scope : Value
    {
        private Dictionary<string, Value> Public { get; set; }
        private Dictionary<string, Value> Private { get; set; }
        private Dictionary<string, Value> Local { get; set; }
        public Scope Parent { get; private set; }

        public override GTType Type
        {
            get { return GTType.Scope; }
        }

        public Scope(Scope parent)
        {
            this.Parent = parent;

            this.Public = new Dictionary<string, Value>();
            this.Private = new Dictionary<string, Value>();
            this.Local = new Dictionary<string, Value>();
        }

        public Scope() : this(null) { }

        public Scope Close()
        {
            // create a new child closure
            Scope s = new Scope(Parent == null ? null : Parent.Close());

            // Duplicate all variables
            foreach (var v in Local)
                s.Add(v.Key, v.Value);

            // Set a reference to the same static scopes
            s.Public = this.Public;
            s.Private = this.Private;

            return s;
        }

        public void Add(string name, Value val, ScopeMode mode = ScopeMode.Local)
        {
            (mode == ScopeMode.Local ? Local :
                mode == ScopeMode.Public ? Public :
                    Private).Add(name, val);
        }

        public bool TryFind(string name, out Value ret, Value newval = null)
        {
            // check for local copy
            if (Local.ContainsKey(name))
            {
                if (newval != null)
                    Local[name] = newval;

                ret = Local[name];
                return true;
            }

            // check for private copy
            if (Private.ContainsKey(name))
            {
                if (newval != null)
                    Private[name] = newval;

                ret = Private[name];
                return true;
            }

            // check for public copy
            if (Public.ContainsKey(name))
            {
                if (newval != null)
                    Public[name] = newval;

                ret = Public[name];
                return true;
            }

            // otherwise recurse upwards
            if (Parent != null)
                return Parent.TryFind(name, out ret, newval);

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
        Local, Public, Private
    }
}
