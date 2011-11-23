using System.Linq;
using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<IExpression>

        EmptyBlock =
            from open in Parse.Char('{')
            from space in Parse.WhiteSpace.Many()
            from close in Parse.Char('}')
            select new Block(),

        FilledBlock =
            from open in Parse.Char('{')
            from s1 in Parse.WhiteSpace.Many()
            from first in Expression.Once()
            from rest in
                (from _ in Parse.WhiteSpace.AtLeastOnce()
                 from e in Expression
                 select e).Many()
            from s2 in Parse.WhiteSpace.Many()
            from close in Parse.Char('}')
            select new Block(first.Concat(rest)),

        SingleBlock =
            from open in Parse.Char('(')
            from s1 in Parse.WhiteSpace.Many()
            from exp in Expression
            from s2 in Parse.WhiteSpace.Many()
            from close in Parse.Char(')')
            select exp,

        Block = EmptyBlock.Or(FilledBlock).Or(SingleBlock);
    }
}
