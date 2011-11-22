using System.Collections.Generic;

namespace GreenTea.Types
{
    public class Function : Value
    {
        public IExpression Body { get; private set; }
        public List<int> Parameters { get; private set; }
        public Scope Container { get; private set; }

        public Function(IExpression body, Scope parent, params string[] args)
        {
            this.Body = body;
            this.Container = parent;
            this.Parameters = new List<int>();

            foreach (var s in args)
                Parameters.Add(s.GetHashCode());
        }

        public override GTType Type
        {
            get { return GTType.Function; }
        }
    }
}
