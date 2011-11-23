using System.Collections.Generic;
using System.Linq;

namespace GreenTea
{
    public class Function : Value
    {
        public IExpression Body { get; private set; }
        public List<int> Parameters { get; private set; }
        public Scope Container { get; private set; }

        public Function(IExpression body, Scope parent, IEnumerable<string> args)
        {
            this.Body = body;
            this.Container = parent;
            this.Parameters = new List<int>(args.Select(s => s.GetHashCode()));
        }

        public override GTType Type
        {
            get { return GTType.Function; }
        }
    }
}
