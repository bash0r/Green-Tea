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
            return String.Format("{0} < {1}", Left, Right);
        }
    }
}
