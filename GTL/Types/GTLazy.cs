using System.Collections.Generic;

namespace GreenTea
{
    public sealed class GTLazy : Value
    {
        public IExpression Body { get; private set; }
        public Scope Container { get; private set; }
        private Value Cache = null;

        public Value Val
        {
            get
            {
                if (Cache == null)
                    Cache = Body.Evaluate(Container);

                return Cache;
            }
        }

        public GTLazy(IExpression exp, Scope scope)
        {
            this.Body = exp;
            this.Container = scope;
        }

        #region Implementation
        public override int Count
        {
            get { return Val.Count; }
        }

        public override Value Add(Value v)
        {
            return Val.Add(v);
        }

        public override IEnumerable<Value> Enumerate()
        {
            return Val.Enumerate();
        }

        public override Value this[int i]
        {
            get { return Val[i]; }
        }

        public override Value Set(int i, Value v)
        {
            return Val.Set(i, v);
        }

        public override Value InsertBefore(int i, Value v)
        {
            return Val.InsertBefore(i, v);
        }

        public override Value InsertAfter(int i, Value v)
        {
            return Val.InsertAfter(i, v);
        }

        public override GTType Type
        {
            get { return Val.Type; }
        }

        public override string ToString()
        {
            return '&' + Val.ToString();
        }
        #endregion
    }
}
