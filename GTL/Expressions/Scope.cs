using System;
using System.Linq;

namespace GreenTea
{
    public class ScopeCreation : IExpression
    {
        public IExpression Body { get; private set; }
        public IExpression Parent { get; private set; }

        public ScopeCreation(IExpression body, IExpression parent)
        {
            this.Body = body;
            this.Parent = parent;
        }

        public Value Evaluate(Scope scope)
        {
            Scope s;

            if (Parent == null)
                s = new Scope(scope, true);
            else
                s = new Scope(from v in Parent.Evaluate(scope).Enumerate()
                              where v.Type == GTType.Scope
                              select (Scope)v, true);

            Body.Evaluate(s); // return value is ignored

            return s;
        }

        public override string ToString()
        {
            return String.Format("scope {0}{1}", Body, (Parent == null ? "" : String.Format(" has {0}", Parent)));
        }
    }
}
