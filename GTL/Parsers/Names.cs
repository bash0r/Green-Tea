using System.Linq;
using Sprache;

namespace GreenTea
{
    internal static partial class ParserPre
    {
        private static Parser<char>

        IdFirst =
            Parse.Letter.XOr(Parse.Char(c => "äöüÄÖÜß_".Contains(c), "Umlaut")),

        IdBody =
            IdFirst.XOr(Parse.Digit);


        internal static Parser<string>

        Identifier =
            from first in IdFirst
            from rest in IdBody.Many().Text()
            let name = first + rest
            where !Reserved.Contains(name)
            select name;

        private static Parser<IExpression>

        VariableUsage =
            from id in ParserPre.Identifier
            select new Usage(id),

        ScopedUsage =
            from scope in ParserPre.Identifier
            from dot in Parse.Char('.')
            from id in ParserPre.Identifier
            select new Usage(id, new Usage(scope));

        internal static Parser<IExpression>

        Variable =
            ScopedUsage.Or(VariableUsage);

        private static readonly string[] Reserved =
        {
            "func", "var", "public", "private", "if", "else", "case", "of", "from", "where", "select", "in", "namespace", "include", "void", "true", "false", "scope", "has"
        };
    }
}
