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

        IfElse =
            from word in Parse.String("if")
            from body in Body
            from alt in Else.Or(Parse.Return(GTVoid.Void))
            select new IfElse(body.First, body.Second, alt),

        Else =
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from word in Parse.String("else")
            from s2 in Parse.WhiteSpace.AtLeastOnce()
            from ex in Expression
            select ex,

        Conditional =
            IfElse;

        private static Parser<ExpPair>

        Body =
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from e1 in Expression
            from s2 in Parse.WhiteSpace.AtLeastOnce()
            from e2 in Expression
            select new ExpPair(e1, e2);

        private struct ExpPair
        {
            public IExpression First, Second;

            public ExpPair(IExpression fst, IExpression snd)
            {
                First = fst;
                Second = snd;
            }
        }
    }
}
