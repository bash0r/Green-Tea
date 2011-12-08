using System;
using System.Collections.Generic;
using System.Text;

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

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (string s in Parameters)
                sb.AppendFormat(", {0}", s);

            if (Parameters.Count > 0)
            {
                sb.Remove(0, 2);
                sb.Insert(0, '(');
                sb.Append(')');
            }

            return String.Format("func{0} {1}", sb, Body);
        }
    }
}
