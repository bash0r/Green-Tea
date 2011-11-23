using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        public static IExpression ParseString(string s)
        {
            return Block.Parse("{" + s + "}");
        }

        // Whitespace
        private static Parser<string> Space = Parse.WhiteSpace.AtLeastOnce().Text();
    }
}
