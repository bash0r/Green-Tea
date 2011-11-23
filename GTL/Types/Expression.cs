using System.Collections.Generic;

namespace GreenTea
{
    public sealed class Expression : Value
    {
        public IExpression Body { get; private set; }
        public Scope Container { get; private set; }
        private Value CacheVal = null;

        private Value Cache
        {
            get
            {
                if (CacheVal == null)
                    CacheVal = Body.Evaluate(Container);

                return CacheVal;
            }
        }

        public Expression(IExpression exp, Scope scope)
        {
            this.Body = exp;
            this.Container = scope;
        }

        #region Implementation
        public override int Count
        {
            get { return Cache.Count; }
        }

        public override Value Add(Value v)
        {
            return Cache.Add(v);
        }

        public override IEnumerable<Value> Enumerate()
        {
            return Cache.Enumerate();
        }

        public override Value this[int i]
        {
            get { return Cache[i]; }
        }

        public override Value Set(int i, Value v)
        {
            return Cache.Set(i, v);
        }

        public override Value InsertBefore(int i, Value v)
        {
            return Cache.InsertBefore(i, v);
        }

        public override Value InsertAfter(int i, Value v)
        {
            return Cache.InsertAfter(i, v);
        }

        public override GTType Type
        {
            get { return GTType.List; }
        }

        public override string ToString()
        {
            return '&' + Cache.ToString();
        }
        #endregion
    }
}
