using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
    public class AndOp : Operator
    {
        public AndOp(IExpression left, IExpression right) : base(left, right) { }

        public override Value Evaluate(Scope scope)
        {
            return new GTBool(Left.Evaluate(scope).IsTrue() && Right.Evaluate(scope).IsTrue());
        }

        public override string ToString()
        {
            return String.Format("{0} && {1}", Left, Right);
        }
    }

    public class OrOp : Operator
    {
        public OrOp(IExpression left, IExpression right) : base(left, right) { }

        public override Value Evaluate(Scope scope)
        {
            return new GTBool(Left.Evaluate(scope).IsTrue() || Right.Evaluate(scope).IsTrue());
        }

        public override string ToString()
        {
            return String.Format("{0} || {1}", Left, Right);
        }
    }
}
