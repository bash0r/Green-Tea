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

        ListLiteral =
            from open in Parse.Char('[')
            from exps in ByVals.Or(Parse.Return(new List<IExpression>()))
            from close in Parse.Char(']')
            select new ListLiteral(exps),

        ListExp =
            ListLiteral;
    }
}
