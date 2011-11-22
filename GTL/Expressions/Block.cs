using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
    public class Block : List<IExpression>, IExpression
    {
        public Block() : base() { }
        public Block(IEnumerable<IExpression> collection) : base(collection) { }

        public Value Evaluate(Scope scope)
        {
            Value ret = GTVoid.Void;

            foreach (var exp in this)
                ret = exp.Evaluate(scope);

            return ret;
        }
    }
}
