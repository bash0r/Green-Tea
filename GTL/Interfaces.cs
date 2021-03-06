﻿using System.Collections.Generic;

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

        public virtual int Size
        {
            get { return 1; }
        }

        public virtual Value Add(Value v)
        {
            return new GTTree(this).Add(v);
        }

        public virtual Value AddRange(Value v)
        {
            return new GTTree(this).AddRange(v);
        }

        public virtual Value AddExp(IExpression exp, Scope scope) // AddLazy
        {
            return new GTTree(this, null, new GTLazy(exp, scope));
        }

        public virtual IEnumerable<Value> Enumerate()
        {
            return new GTTree(this).Enumerate();
        }

        public virtual Value this[int i]
        {
            get { return this; }
        }

        public virtual Value Set(int i, Value val)
        {
            return new GTTree(this).Set(i, val);
        }

        public virtual Value InsertBefore(int i, Value val)
        {
            return new GTTree(this).InsertBefore(i, val);
        }

        public virtual Value InsertAfter(int i, Value val)
        {
            return new GTTree(this).InsertAfter(i, val);
        }

        public Value Evaluate(Scope scope)
        {
            return this;
        }

        internal virtual Value Self()
        {
            return this;
        }
    }
}
