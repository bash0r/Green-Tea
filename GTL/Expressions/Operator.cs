using System;

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

        public override Value Evaluate(Scope scope)
        {
            Value l = Left.Evaluate(scope);
            Value r = Right.Evaluate(scope);

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
    }
}
