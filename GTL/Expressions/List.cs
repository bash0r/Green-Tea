using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
    public class ListLiteral : IExpression
    {
        public List<IExpression> Expressions { get; private set; }

        public ListLiteral(IEnumerable<IExpression> exps)
        {
            this.Expressions = new List<IExpression>(exps);
        }

        public Value Evaluate(Scope scope)
        {
            return new GTTree(Expressions.Select(e => e.Evaluate(scope)));
        }

        public override string ToString()
        {
            StringBuilder args = new StringBuilder();

            foreach (var e in Expressions)
                args.AppendFormat(", {0}", e);

            if (args.Length > 0)
                args.Remove(0, 2);

            return String.Format("[{0}]", args);
        }
    }
}
