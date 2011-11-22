using System;
using System.Collections.Generic;

namespace GreenTea
{
    public interface IExpression
    {
        Value Evaluate(Scope scope);
    }

    public abstract class Value : IExpression
    {
        public abstract GTType Type { get; }

        public Value Evaluate(Scope scope)
        {
            return this;
        }
    }
}
