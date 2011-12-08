using System;
using System.Linq;
using System.Collections.Generic;

namespace GreenTea
{
    public abstract class Operator : IExpression
    {
        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }

        public abstract Value Evaluate(Scope scope);

        public Operator(IExpression left, IExpression right)
        {
            this.Left = left;
            this.Right = right;
        }
    }

    public abstract class OpAdapter : Operator
    {
        protected abstract Func<dynamic, dynamic, dynamic> Fun { get; }
        protected OpAdapter(IExpression left, IExpression right) : base(left, right) { }

        public Value Evaluate(IExpression left, IExpression right, Scope scope)
        {
            Value l = left.Evaluate(scope);
            Value r = right.Evaluate(scope);

            if (l.Count > 1 && l.Count == r.Count)
            {
                var lv = new List<Value>(l.Enumerate());
                var rv = new List<Value>(r.Enumerate());
                var res = new List<Value>(lv.Count);

                for (int i = 0; i < lv.Count(); i++)
                    res.Add(Evaluate(lv[i], rv[i], scope));

                return new GTTree(res);
            }

            if (l.Count > 1 && r.Count == 1)
                return new GTTree(from x in l.Enumerate()
                                  select Evaluate(x, r, scope));

            if (r.Count > 1 && l.Count == 1)
                return new GTTree(from x in r.Enumerate()
                                  select Evaluate(l, x, scope));

            if (l is IGTAdapter && r is IGTAdapter)
            {
                dynamic res = Fun.Invoke(((IGTAdapter)l).Get(), ((IGTAdapter)r).Get());

                if (l.Type == GTType.Float || r.Type == GTType.Float)
                    return new GTFloat(res);
                else
                    return new GTInt(res);
            }

            throw new InvalidOperationException(String.Format("Cannot use operator {0} on types {1} and {2}", this, l.Type, r.Type));
        }

        public override Value Evaluate(Scope scope)
        {
            return Evaluate(Left, Right, scope);
        }
    }
}
