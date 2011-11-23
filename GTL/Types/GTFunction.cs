using System.Collections.Generic;

namespace GreenTea
{
    public class GTFunction : Value
    {
        public IExpression Body { get; private set; }
        public List<string> Parameters { get; private set; }
        public Scope Container { get; private set; }

        public GTFunction(IExpression body, Scope parent, IEnumerable<string> args)
        {
            this.Body = body;
            this.Container = parent;
            this.Parameters = new List<string>(args);
        }

        public override GTType Type
        {
            get { return GTType.Function; }
        }
    }
}
