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

        public virtual int Count
        {
            get { return 1; }
        }

        public virtual Value Add(Value v)
        {
            return new Tree(this).Add(v);
        }

        public virtual Value AddExp(IExpression exp, Scope scope) // AddLazy
        {
            return new Tree(this, null, new Expression(exp, scope));
        }

        public virtual IEnumerable<Value> Enumerate()
        {
            return new Tree(this).Enumerate();
        }

        public virtual Value this[int i]
        {
            get { return this; }
        }

        public virtual Value Set(int i, Value val)
        {
            return new Tree(this).Set(i, val);
        }

        public virtual Value InsertBefore(int i, Value val)
        {
            return new Tree(this).InsertBefore(i, val);
        }

        public virtual Value InsertAfter(int i, Value val)
        {
            return new Tree(this).InsertAfter(i, val);
        }

        public Value Evaluate(Scope scope)
        {
            return this;
        }
    }
}
