using Sprache;
using System;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<IExpression>

        IfElse =
            from word in Parse.String("if")
            from body in Body
            from alt in Else.Or(Parse.Return(GTVoid.Void))
            select new IfElse(body.Item1, body.Item2, alt),

        Else =
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from word in Parse.String("else")
            from s2 in Parse.WhiteSpace.AtLeastOnce()
            from ex in Expression
            select ex,

        CaseOf =
            from word in Parse.String("case")
            from opts in Of.AtLeastOnce()
            from alt in Else.Or(Parse.Return(GTVoid.Void))
            select new CaseOf(opts, alt),

        Conditional =
            IfElse.Or(CaseOf);


        private static Parser<Tuple<IExpression, IExpression>>

        Body =
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from e1 in Expression
            from s2 in Parse.WhiteSpace.AtLeastOnce()
            from e2 in Expression
            select new Tuple<IExpression, IExpression>(e1, e2),

        Of =
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from word in Parse.String("of")
            from body in Body
            select body;
    }
}
