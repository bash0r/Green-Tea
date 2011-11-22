using System.Collections.Generic;

namespace GreenTea
{
    public interface IExpression
    {
        Value Evaluate(Scope scope);
    }

    public interface GTList
    {
        int Count { get; }
        GTList Add(Value v);
        IEnumerable<Value> Enumerate();
        Value this[int i] { get; }
        GTList Set(int i, Value val);
        GTList InsertBefore(int i, Value val);
        GTList InsertAfter(int i, Value val);
    }

    public static class GTListExt
    {
        public static GTList AddExp(this GTList list, IExpression exp, Scope scope) // AddLazy
        {
            return new TreeList(null, list, new ExpressionList(exp, scope));
        }
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
