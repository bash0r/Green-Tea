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

        FunctionCreation =
            from word in Parse.String("func")
            from args in
                ArgsParser.Or(
                    from sp in Space
                    select new List<string>())
            from body in Expression
            select new FunctionCreation(body, args),

        ByVal =
            from s1 in Space.Many()
            from exp in Expression
            from s2 in Space.Many()
            select exp,

        FunctionApplication =
            from word in Parse.Char(':')
            from s1 in Space
            from fun in Expression
            from s2 in Space
            from open in Parse.Char('(')
            from args in ByVals.Or(Parse.Return(new List<IExpression>()))
            from close in Parse.Char(')')
            select new FunctionApplication(fun, args);


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
            from s1 in Space.Many()
            from open in Parse.Char('(')
            from first in Param.Once()
            from rest in
                (from comma in Parse.Char(',')
                 from param in Param
                 select param).Many()
            from close in Parse.Char(')')
            select first.Concat(rest);


        private static Parser<string>

        Param =
            from s1 in Space.Many()
            from id in Identifier
            from s2 in Space.Many()
            select id;
    }
}
