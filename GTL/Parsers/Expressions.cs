using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<IExpression> Expression = null;

        public static void AddExpression(Parser<IExpression> p)
        {
            if (Expression == null)
                Expression = p;

            else
                Expression = Expression.Or(p);
        }

        static Parser()
        {
            AddExpression(Literal);
            AddExpression(VariableExp);
            AddExpression(Block);
        }
    }
}
