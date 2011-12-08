using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<IExpression>

        FunctionCreation =
            from word in Parse.String("func")
            from args in
                ArgsParser.Or(
                    from sp in Parse.WhiteSpace.AtLeastOnce()
                    select new List<string>())
            from body in SimpleExpression
            select new FunctionCreation(body, args),

        ByVal =
            from s1 in Parse.WhiteSpace.Many()
            from exp in Expression
            from s2 in Parse.WhiteSpace.Many()
            select exp,

        FunctionApplication =
            from fun in
                (from word in Parse.Char(':')
                 from s1 in Parse.WhiteSpace.Many()
                 from ex in Expression
                 select ex).Or(
                 from name in ParserPre.Identifier
                 select new Usage(name))
            from s2 in Parse.WhiteSpace.Many()
            from open in Parse.Char('(')
            from args in ByVals.Or(Parse.Return(new List<IExpression>()))
            from close in Parse.Char(')')
            select new FunctionApplication(fun, args),

        FunctionExp =
            FunctionCreation.Or(FunctionApplication);


        private static Parser<IEnumerable<IExpression>>

        ByVals =
            from first in ByVal.Once()
            from rest in
                (from comma in Parse.Char(',')
                 from val in ByVal
                 select val).Many()
            select first.Concat(rest);


        private static Parser<IEnumerable<string>>

        ArgsParser =
            from s1 in Parse.WhiteSpace.Many()
            from open in Parse.Char('(')
            from first in Param.Once()
            from rest in
                (from comma in Parse.Char(',')
                 from param in Param
                 select param).Many()
            from close in Parse.Char(')')
            from s2 in Parse.WhiteSpace.Many()
            select first.Concat(rest);


        private static Parser<string>

        Param =
            from s1 in Parse.WhiteSpace.Many()
            from id in ParserPre.Identifier
            from s2 in Parse.WhiteSpace.Many()
            select id;
    }
}
