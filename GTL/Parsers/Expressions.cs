using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<IExpression> Expression = null;

        public static void AddExpression(Parser<IExpression> p)
        {
            if (Expression == null)
                Expression = p;

            else
                Expression = Expression.Or(p);
        }

        static Parser()
        {
            AddExpression(Literal);
            AddExpression(Block);
            AddExpression(Conditional);
            AddExpression(FunctionExp);
            AddExpression(VariableExp);
        }
    }
}
