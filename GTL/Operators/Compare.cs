using System;

namespace GreenTea
{
    public class EqOperator : Operator
    {
        public EqOperator(IExpression left, IExpression right) : base(left, right) { }

        public override Value Evaluate(Scope scope)
        {
            return new GTBool(Left.Evaluate(scope).CompareTo(Right.Evaluate(scope)) == 0);
        }

        public override string ToString()
        {
            return String.Format("{0} == {1}", Left, Right);
        }
    }

    public class NeqOperator : Operator
    {
        public NeqOperator(IExpression left, IExpression right) : base(left, right) { }

        public override Value Evaluate(Scope scope)
        {
            return new GTBool(Left.Evaluate(scope).CompareTo(Right.Evaluate(scope)) != 0);
        }

        public override string ToString()
        {
            return String.Format("{0} != {1}", Left, Right);
        }
    }

    public class LTOperator : Operator
    {
        public LTOperator(IExpression left, IExpression right) : base(left, right) { }

        public override Value Evaluate(Scope scope)
        {
            return new GTBool(Left.Evaluate(scope).CompareTo(Right.Evaluate(scope)) == -1);
        }

        public override string ToString()
        {
            return String.Format("{0} < {1}", Left, Right);
        }
    }

    public class GTOperator : Operator
    {
        public GTOperator(IExpression left, IExpression right) : base(left, right) { }

        public override Value Evaluate(Scope scope)
        {
            return new GTBool(Left.Evaluate(scope).CompareTo(Right.Evaluate(scope)) == 1);
        }

        public override string ToString()
        {
            return String.Format("{0} > {1}", Left, Right);
        }
    }

    public class LTEOperator : Operator
    {
        public LTEOperator(IExpression left, IExpression right) : base(left, right) { }

        public override Value Evaluate(Scope scope)
        {
            int res = Left.Evaluate(scope).CompareTo(Right.Evaluate(scope));

            return new GTBool(res == -1 || res == 0); // can't be -2
        }

        public override string ToString()
        {
            return String.Format("{0} <= {1}", Left, Right);
        }
    }

    public class GTEOperator : Operator
    {
        public GTEOperator(IExpression left, IExpression right) : base(left, right) { }

        public override Value Evaluate(Scope scope)
        {
            return new GTBool(Left.Evaluate(scope).CompareTo(Right.Evaluate(scope)) >= 0);
        }

        public override string ToString()
        {
            return String.Format("{0} >= {1}", Left, Right);
        }
    }
}
