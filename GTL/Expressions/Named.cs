using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
    public class NamedExpression : IExpression
    {
        public IExpression Body { get; private set; }
        public string Format { get; private set; }

        public NamedExpression(IExpression body, string format)
        {
            this.Body = body;
            this.Format = format;
        }

        public Value Evaluate(Scope scope)
        {
            return Body.Evaluate(scope);
        }

        public override string ToString()
        {
            return String.Format(Format, Body);
        }
    }
}
