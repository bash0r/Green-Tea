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

        EmptyBlock =
            from open in Parse.Char('{')
            from space in Space.Many()
            from close in Parse.Char('}')
            select new Block(),

        FilledBlock =
            from open in Parse.Char('{')
            from s1 in Space.Many()
            from first in Expression.Once()
            from rest in
                (from _ in Space
                 from e in Expression
                 select e).Many()
            from s2 in Space.Many()
            from close in Parse.Char('}')
            select new Block(first.Concat(rest)),

        Block = EmptyBlock.Or(FilledBlock);
    }
}
