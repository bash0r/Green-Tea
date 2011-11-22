using System.Collections.Generic;

namespace GreenTea
{
    public sealed class ExpressionList : GTList
    {
        public IExpression Expression { get; private set; }
        public Scope Container { get; private set; }
        private GTList CacheVal = null;

        private GTList Cache
        {
            get
            {
                if (CacheVal == null)
                    CacheVal = (GTList)Expression.Evaluate(Container);

                return CacheVal;
            }
        }

        public ExpressionList(IExpression exp, Scope scope)
        {
            this.Expression = exp;
            this.Container = scope;
        }

        #region Implementation
        public int Count
        {
            get { return Cache.Count; }
        }

        public GTList Add(Value v)
        {
            return Cache.Add(v);
        }

        public IEnumerable<Value> Enumerate()
        {
            return Cache.Enumerate();
        }

        public Value this[int i]
        {
            get { return Cache[i]; }
        }

        public GTList Set(int i, Value v)
        {
            return Cache.Set(i, v);
        }

        public GTList InsertBefore(int i, Value v)
        {
            return Cache.InsertBefore(i, v);
        }

        public GTList InsertAfter(int i, Value v)
        {
            return Cache.InsertAfter(i, v);
        }

        public override string ToString()
        {
            return '&' + Cache.ToString();
        }
        #endregion
    }
}
