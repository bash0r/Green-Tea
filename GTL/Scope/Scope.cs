using System.Collections.Generic;
using System;
using System.Text;

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

        public bool HasStatic
        {
            get { return Public != null && Private != null; }
        }

        public Scope(Scope parent, bool HasStatic = false)
        {
            this.Parent = parent;
            this.Local = new Dictionary<string, Value>();

            if (HasStatic)
            {
                this.Public = new Dictionary<string, Value>();
                this.Private = new Dictionary<string, Value>();
            }
        }

        public Scope() : this(null) { }

        public Scope Close()
        {
            // create a new child closure
            Scope s = new Scope(Parent == null ? null : Parent.Close());

            // Duplicate all variables
            foreach (var v in Local)
                s.Add(v.Key, v.Value);

            return s;
        }

        public void Add(string name, Value val, ScopeMode mode = ScopeMode.Local)
        {
            switch (mode)
            {
                case ScopeMode.Local:
                    Local.Add(name, val);
                    break;

                case ScopeMode.Public:
                case ScopeMode.Private:
                    if (HasStatic)
                        (mode == ScopeMode.Public ? Public : Private).Add(name, val);
                    else
                        if (Parent != null)
                            Parent.Add(name, val, mode);
                        else
                            throw new InvalidOperationException("Cannot add a static variable to an orphan scope");
                    break;
            }
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

            if (HasStatic)
            {
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

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("scope {");

            if (HasStatic)
            {
                foreach (var p in Public)
                    sb.AppendFormat("public {0} = {1}\n", p.Key, p.Value);
                sb.AppendLine();

                foreach (var p in Private)
                    sb.AppendFormat("private {0} = {1}\n", p.Key, p.Value);
                sb.AppendLine();
            }

            foreach (var l in Local)
                sb.AppendFormat("var {0} = {1}\n", l.Key, l.Value);

            sb.Append("}");

            if (Parent != null)
                sb.AppendFormat(" has {0}", Parent);

            return sb.ToString();
        }
    }

    public enum ScopeMode
    {
        Local, Public, Private
    }
}
