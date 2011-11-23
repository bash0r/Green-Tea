using System;
using System.Collections.Generic;

namespace GreenTea
{
    public class GTVoid : Value
    {
        public static readonly GTVoid Void = new GTVoid();

        public override GTType Type
        {
            get { return GTType.Void; }
        }

        public GTVoid() { }

        #region Implementations
        public override int Count
        {
            get { return 0; }
        }

        public override Value Add(Value v)
        {
            return new GTTree(v);
        }

        public override IEnumerable<Value> Enumerate()
        {
            return new GTTree().Enumerate();
        }

        public override Value this[int i]
        {
            get { throw new IndexOutOfRangeException(); }
        }

        public override Value Set(int i, Value v)
        {
            throw new IndexOutOfRangeException();
        }

        public override Value InsertBefore(int i, Value v)
        {
            throw new IndexOutOfRangeException();
        }

        public override Value InsertAfter(int i, Value v)
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
