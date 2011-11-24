using System;
using System.Collections.Generic;
using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<IExpression>

        ExpressionCache =
            null,

        Lazy =
            from lead in Parse.Char('&')
            from exp in Expression
            select new Lazy(exp);

        private static Parser<IExpression> Expression
        {
            get { return ExpressionCache ?? RebuildExp(); }
        }

        private static List<Parser<IExpression>> ExpressionList = new List<Parser<IExpression>>();
        private static Dictionary<int, List<Tuple<string, Func<IExpression, IExpression, Operator>>>> OperatorList = new Dictionary<int, List<Tuple<string, Func<IExpression, IExpression, Operator>>>>();

        public static void AddExpression(Parser<IExpression> p)
        {
            ExpressionList.Add(p);
            ExpressionCache = null;
        }

        public static void AddOperator(int prec, string name, Func<IExpression, IExpression, Operator> op)
        {
            if (!OperatorList.ContainsKey(prec))
                OperatorList[prec] = new List<Tuple<string, Func<IExpression, IExpression, Operator>>>();

            OperatorList[prec].Add(new Tuple<string,Func<IExpression,IExpression,Operator>>(name, op));
            ExpressionCache = null;
        }

        private static Parser<IExpression> RebuildExp()
        {
            foreach (var v in ExpressionList)
                if (ExpressionCache == null)
                    ExpressionCache = v;
                else
                    ExpressionCache = ExpressionCache.Or(v);

            // sort by precedence
            List<int> precs = new List<int>(OperatorList.Keys);
            precs.Sort();

            foreach (var k in precs)
            {
                Parser<Func<IExpression, IExpression, Operator>> cache = null;

                foreach (var op in OperatorList[k])
                {
                    var p = from symbol in Parse.String(op.Item1).Text()
                            select op.Item2;

                    if (cache == null)
                        cache = p;
                    else
                        cache = cache.Or(p);
                }

                ExpressionCache = Parse.ChainOperator(cache, ExpressionCache, (op, left, right) => op(left, right));
            }

            return ExpressionCache;
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
