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

        VoidLiteral =
            from word in Parse.String("void")
            select new GTVoid(),

        IntLiteral =
            from sign in Parse.Char('-').XOr(Parse.Return(' '))
            from digits in Parse.Digit.AtLeastOnce().Text()
            select new GTInt(long.Parse(sign + digits)),

        FloatLiteral =
            from sign in Parse.Char('-').XOr(Parse.Return(' '))
            from ipart in Parse.Digit.AtLeastOnce().Text()
            from dot in Parse.Char('.')
            from fpart in Parse.Digit.AtLeastOnce().Text()
            select new GTFloat(double.Parse(sign + ipart + dot + fpart)),

        BoolLiteral =
            from val in Parse.String("true").XOr(Parse.String("false")).Text()
            select new GTBool(bool.Parse(val)),

        StringLiteral =
            from open in Parse.Char('"')
            from body in Parse.CharExcept('"').Many().Text() // TODO: add escaping
            from close in Parse.Char('"')
            select new GTString(body),

        Literal =
            VoidLiteral.XOr(BoolLiteral).XOr(FloatLiteral).XOr(IntLiteral).XOr(StringLiteral);
    }
}
