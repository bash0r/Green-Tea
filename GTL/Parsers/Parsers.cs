using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        public static IExpression ParseString(string s)
        {
            return Block.End().Parse("{" + s + "}");
        }
    }
}
