using System;
using System.Collections.Generic;

namespace GreenTea
{
    public class GTVoid : Value, GTList
    {
        public static readonly GTVoid Void = new GTVoid();

        public override GTType Type
        {
            get { return GTType.Void; }
        }

        public GTVoid() { }

        #region Implementations
        public int Count
        {
            get { return 0; }
        }

        public GTList Add(Value v)
        {
            return new TreeList(v);
        }

        public IEnumerable<Value> Enumerate()
        {
            return new TreeList().Enumerate();
        }

        public Value this[int i]
        {
            get { throw new IndexOutOfRangeException(); }
        }

        public GTList Set(int i, Value v)
        {
            throw new IndexOutOfRangeException();
        }

        public GTList InsertBefore(int i, Value v)
        {
            throw new IndexOutOfRangeException();
        }

        public GTList InsertAfter(int i, Value v)
        {
            throw new IndexOutOfRangeException();
        }

        public override string ToString()
        {
            return "void";
        }
        #endregion
    }
}
