﻿using System.Collections.Generic;
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
            Scope ex = new Scope(scope);

            foreach (var exp in this)
                ret = exp.Evaluate(ex);

            return ret;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");

            foreach (var ex in this)
                sb.AppendLine(ex.ToString());

            sb.Append("}");
            return sb.ToString();
        }
    }
}
