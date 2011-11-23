using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<IExpression>

        Expression =
            null,

        Lazy =
            from lead in Parse.Char('&')
            from exp in Expression
            select new Lazy(exp);

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
            AddExpression(Lazy);
            AddExpression(Block);
            AddExpression(Conditional);
            AddExpression(Select);
            AddExpression(FunctionExp);
            AddExpression(VariableExp);
            AddExpression(ListExp);
        }
    }
}
