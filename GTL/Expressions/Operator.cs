using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
