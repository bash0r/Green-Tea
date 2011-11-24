using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
    internal static partial class ValueExt
    {
        internal static bool IsNumber(this Value v)
        {
            return (v.Type == GTType.Float || v.Type == GTType.Integer);
        }

        internal static dynamic AsNumber(this Value v)
        {
            switch (v.Type)
            {
                case GTType.Float:
                    return ((GTFloat)v).Value;

                case GTType.Integer:
                    return ((GTInt)v).Value;

                case GTType.Bool:
                    return ((GTBool)v).Value ? 1 : 0;

                default:
                    return 0;
            }
        }
    }

    public class AddOperator : Operator
    {
        public AddOperator(IExpression left, IExpression right) : base(left, right) { }

        public override Value Evaluate(Scope scope)
        {
            return Left.Evaluate(scope).AsNumber() + Right.Evaluate(scope).AsNumber();
        }
    }
}
