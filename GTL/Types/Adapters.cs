using System;
using System.Collections.Generic;

namespace GreenTea
{
    public abstract class GTAdapter<T> : Value, GTList
    {
        public T Value { get; set; }

        public GTAdapter(T val)
        {
            this.Value = val;
        }

        #region Implementations
        public int Count
        {
            get { return 1; }
        }

        public GTList Add(Value v)
        {
            return new TreeList(this).Add(v);
        }

        public IEnumerable<Value> Enumerate()
        {
            return new TreeList(this).Enumerate();
        }

        public Value this[int i]
        {
            get
            {
                return new TreeList(this)[i];
            }
        }

        public GTList Set(int i, Value v)
        {
            return new TreeList(this).Set(i, v);
        }

        public GTList InsertBefore(int i, Value v)
        {
            return new TreeList(this).InsertBefore(i, v);
        }

        public GTList InsertAfter(int i, Value v)
        {
            return new TreeList(this).InsertAfter(i, v);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    #endregion
    }

    public class GTBool : GTAdapter<Boolean>
    {
        public override GTType Type
        {
            get { return GTType.Bool; }
        }

        public GTBool(Boolean v) : base(v) { }
    }

    public class GTInt : GTAdapter<Int64>
    {
        public override GTType Type
        {
            get { return GTType.Integer; }
        }

        public GTInt(Int64 v) : base(v) { }
    }

    public class GTFloat : GTAdapter<Double>
    {
        public override GTType Type
        {
            get { return GTType.Float; }
        }

        public GTFloat(Double v) : base(v) { }
    }

    public class GTString : GTAdapter<String>
    {
        public override GTType Type
        {
            get { return GTType.String; }
        }

        public GTString(String v) : base(v) { }

        public override string ToString()
        {
            return '"' + Value + '"';
        }
    }
}
