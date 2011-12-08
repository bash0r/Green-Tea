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

        SimpleExpressionCache =
            null,

        Lazy =
            from lead in Parse.Char('&')
            from exp in Expression
            select new Lazy(exp);

        private static Parser<IExpression> Expression
        {
            get { return ExpressionCache ?? RebuildExp(); }
        }

        private static Parser<IExpression> SimpleExpression
        {
            get { return SimpleExpressionCache ?? RebuildExp(true); }
        }

        private static Dictionary<int, List<Parser<IExpression>>> ExpressionList = new Dictionary<int, List<Parser<IExpression>>>();
        private static Dictionary<int, List<Tuple<string, Func<IExpression, IExpression, Operator>>>> OperatorList = new Dictionary<int, List<Tuple<string, Func<IExpression, IExpression, Operator>>>>();

        public static void AddExpression(int prec, Parser<IExpression> p)
        {
            if (!ExpressionList.ContainsKey(prec))
                ExpressionList[prec] = new List<Parser<IExpression>>();

            ExpressionList[prec].Add(p);
            ExpressionCache = null;
            SimpleExpressionCache = null;

            RebuildExp();
        }

        public static void AddOperator(int prec, string name, Func<IExpression, IExpression, Operator> op)
        {
            if (!OperatorList.ContainsKey(prec))
                OperatorList[prec] = new List<Tuple<string, Func<IExpression, IExpression, Operator>>>();

            OperatorList[prec].Add(new Tuple<string,Func<IExpression,IExpression,Operator>>(name, op));
            ExpressionCache = null;
            SimpleExpressionCache = null;
        }

        private static Parser<IExpression> RebuildExp(bool simple = false)
        {
            // sort by precedence
            var precs = new List<int>(ExpressionList.Keys);

            foreach (var k in OperatorList.Keys)
                if (!ExpressionList.ContainsKey(k))
                    precs.Add(k);

            // descending
            precs.Sort((a, b) => b.CompareTo(a));

            foreach (var k in precs)
            {
                // expressions
                if (ExpressionList.ContainsKey(k))
                    foreach (var v in ExpressionList[k])
                        if (ExpressionCache == null)
                        {
                            ExpressionCache = v;
                            SimpleExpressionCache = v;
                        }
                        else
                        {
                            // Mirror all changes into the simple expression
                            ExpressionCache = ExpressionCache.Or(v);
                            SimpleExpressionCache = SimpleExpressionCache.Or(v);
                        }

                // operators
                if (OperatorList.ContainsKey(k))
                {
                    Parser<Func<IExpression, IExpression, Operator>> cache = null;

                    foreach (var op in OperatorList[k])
                    {
                        var c = op; // closure
                        var p = //from s1 in Parse.Char('_')
                                from symbol in Parse.String(c.Item1)
                                //from s2 in Parse.Char('_')
                                select c.Item2;

                        if (cache == null)
                            cache = p;
                        else
                            cache = cache.Or(p);
                    }

                    ExpressionCache = Parse.ChainOperator(cache, ExpressionCache.Token(), (op, left, right) => op(left, right));
                }
            }

            return ExpressionCache;
        }

        static Parser()
        {
            AddExpression(0, Block);
            AddExpression(0, ListExp);
            AddExpression(1, Literal);
            AddExpression(2, VariableExp);
            AddExpression(3, Select);
            AddExpression(3, Conditional);
            AddExpression(4, FunctionExp);
            AddExpression(5, Lazy);
            AddExpression(6, ScopeCreation);

            //AddOperator(-1, "^", (l, r) => new DynamicOperator(l, r, (a, b) => Math.Pow(a, b)));

            AddOperator(-2, "*", (l, r) => new MulOperator(l, r));
            AddOperator(-2, "/", (l, r) => new DivOperator(l, r));
            AddOperator(-2, "%", (l, r) => new ModOperator(l, r));

            AddOperator(-3, "+", (l, r) => new AddOperator(l, r));
            AddOperator(-3, "-", (l, r) => new SubOperator(l, r));

            
            AddOperator(-10, "<=", (l, r) => new LTEOperator(l, r));
            AddOperator(-10, ">=", (l, r) => new GTEOperator(l, r));
            AddOperator(-10, "<", (l, r) => new LTOperator(l, r));
            AddOperator(-10, ">", (l, r) => new GTOperator(l, r));

            AddOperator(-11, "==", (l, r) => new EqOperator(l, r));
            AddOperator(-11, "!=", (l, r) => new NeqOperator(l, r));

            AddOperator(-15, "&&", (l, r) => new AndOp(l, r));
            AddOperator(-15, "||", (l, r) => new OrOp(l, r));

            AddOperator(-20, "~<<", (l, r) => new ListAddRange(l, r));
            AddOperator(-20, "~<", (l, r) => new ListAdd(l, r));
        }
    }
}
