using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
    public static partial class Functions
    {
        public static Value add(Value left, Value right)
        {
            return new AddOperator(left, right).Evaluate(null);
        }

        public static Value sub(Value left, Value right)
        {
            return new SubOperator(left, right).Evaluate(null);
        }

        public static Value mul(Value left, Value right)
        {
            return new MulOperator(left, right).Evaluate(null);
        }

        public static Value div(Value left, Value right)
        {
            return new DivOperator(left, right).Evaluate(null);
        }

        public static Value mod(Value left, Value right)
        {
            return new ModOperator(left, right).Evaluate(null);
        }
    }
}
