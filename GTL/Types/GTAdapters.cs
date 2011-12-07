using System;
using System.Collections.Generic;

namespace GreenTea
{
    internal interface IGTAdapter
    {
        dynamic Get();
    }

    public abstract class GTAdapter<T> : Value, IGTAdapter
    {
        public T Value { get; set; }

        public GTAdapter(T val)
        {
            this.Value = val;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public dynamic Get()
        {
            return Value;
        }
    }

    public class GTBool : GTAdapter<Boolean>
    {
        public override GTType Type
        {
            get { return GTType.Bool; }
        }

        public GTBool(Boolean v) : base(v) { }

        public override string ToString()
        {
            return base.ToString().ToLower();
        }
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

        public override int Count
        {
            get { return Value.Length; }
        }
    }
}
