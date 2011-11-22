using System.Collections.Generic;

namespace GreenTea
{
    public class Scope
    {
        public Dictionary<int, Value> Variables { get; private set; }
        public Scope Parent { get; private set; }
        public Module Namespace { get; private set; }

        public Scope(Scope parent, Module mod)
        {
            this.Parent = parent;
            this.Namespace = mod;

            this.Variables = new Dictionary<int, Value>();
        }
    }
}
