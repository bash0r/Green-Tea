using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        /*
         * TODO: Add preparser step:
         * 
         * 1. Strip all comments
         * 2. Unescape strings and replace " with \0
         * 3. Remove all whitespace around operators to prevent parsing issues
         * 4. Parse and remove preparser directives
         * 
         */

        public static IExpression ParseString(string s)
        {
            return Expression.End().Parse(s);
        }

        /*
         * TODO: Add postparser step:
         * 
         * 1. Resolve includes
         * 2. Resolve syntax drawins
         * 3. Resolve preparser directives
         * 
         */
    }
}
