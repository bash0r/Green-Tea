using System.Linq;
using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<SelectExpr>

        From =
            from w1 in Parse.String("from")
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from name in ParserPre.Identifier
            from s2 in Parse.WhiteSpace.AtLeastOnce()
            from w2 in Parse.String("in")
            from s3 in Parse.WhiteSpace.AtLeastOnce()
            from expr in Expression
            from s4 in Parse.WhiteSpace.AtLeastOnce()
            select new FromIn(name, expr),

        Where =
            from word in Parse.String("where")
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from expr in Expression
            from s2 in Parse.WhiteSpace.AtLeastOnce()
            select new Where(expr),

        SelectBody =
            from word in Parse.String("select")
            from s in Parse.WhiteSpace.AtLeastOnce()
            from expr in SimpleExpression
            select new SelectBody(expr);


        private static Parser<IExpression>

        Select =
            from first in From.Once()
            from rest in From.XOr(Where).Many()
            from last in SelectBody.Once()
            select new Select(first.Concat(rest).Concat(last));
    }
}
