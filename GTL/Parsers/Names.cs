using System.Linq;
using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<char>
        
        IdFirst =
            Parse.Letter.XOr(Parse.Char(c => "äöüÄÖÜß_".Contains(c), "Umlaut")),

        IdBody =
            IdFirst.XOr(Parse.Digit);


        private static Parser<string>

        Identifier =
            from first in IdFirst
            from rest in IdBody.Many().Text()
            let name = first + rest
            where !Reserved.Contains(name)
            select name;

        private static readonly string[] Reserved =
        {
            "func", "var", "export", "static", "if", "else", "case", "of", "from", "where", "select", "in", "namespace", "include", "void", "true", "false"
        };
    }
}
