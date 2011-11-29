using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<IExpression>

        ScopeCreation =
            from word in Parse.String("scope")
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from expr in Expression
            from par in ScopeParent.Or(Parse.Return<IExpression>(null))
            select new ScopeCreation(expr, par),

        ScopeParent =
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from word in Parse.String("has")
            from s2 in Parse.WhiteSpace.AtLeastOnce()
            from expr in Expression
            select expr;
    }
}
