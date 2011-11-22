using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
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

    public sealed class TreeList : Value, GTList
    {
        private Value Value;
        private GTList Left;
        private GTList Right;

        #region Constructors
        public TreeList() { }

        public TreeList(Value val) : this()
        {
            this.Value = val;
            this.CountCache = 1;
        }

        public TreeList(Value val, GTList left, GTList right)
        {
            this.Value = val;
            this.Left = left;
            this.Right = right;
            this.CountCache = null;
        }

        public TreeList(IEnumerable<Value> list) : this()
        {
            var v = this;
            
            foreach (Value ex in list)
                v = (TreeList)v.Add(ex);

            // fake mutator
            this.Value = v.Value;
            this.Left = v.Left;
            this.Right = v.Right;

            this.CountCache = list.Count();
        }

        public TreeList(params Value[] list) : this((IEnumerable<Value>)list) { }
        #endregion

        #region Caching
        private int? CountCache = 0;
        private int? LeftCache;
        private int? RightCache;
        private int? ThisCache;

        public int Count
        {
            get
            {
                if (CountCache == null)
                    CountCache = (Value == null ? 0 : 1) + (Left ?? GTVoid.Void).Count + (Right ?? GTVoid.Void).Count;

                return CountCache.Value;
            }
        }

        private int LeftCount
        {
            get
            {
                if (LeftCache == null)
                    LeftCache = (Left ?? GTVoid.Void).Count;

                return LeftCache.Value;
            }
        }

        private int RightCount
        {
            get
            {
                if (RightCache == null)
                    RightCache = (Right ?? GTVoid.Void).Count;

                return RightCache.Value;
            }
        }

        private int ThisCount
        {
            get
            {
                if (ThisCache == null)
                    ThisCache = (Value == null ? 0 : 1);

                return ThisCache.Value;
            }
        }
        #endregion

        #region Modification
        public GTList Add(Value v)
        {
            if (Right == null)
                if (Value == null)
                    return new TreeList(v, Left, Right);
                else
                    return new TreeList(Value, Left, new TreeList(v));
            else
                return new TreeList(Value, Left, Right.Add(v));
        }

        public GTList Set(int i, Value v)
        {
            if (i < LeftCount)
                return new TreeList(Value, Left.Set(i, v), Right);

            else if (i == LeftCount && ThisCount != 0)
                return new TreeList(v, Left, Right);

            else if (RightCount == 0)
                throw new IndexOutOfRangeException();

            else
                return new TreeList(Value, Left, Right.Set(i - LeftCount - ThisCount, v));
        }

        public GTList InsertBefore(int i, Value v)
        {
            if (i < LeftCount)
                return new TreeList(Value, Left.InsertBefore(i, v), Right);

            else if (i == LeftCount && ThisCount != 0)
            {
                if (Left == null)
                    return new TreeList(Value, new TreeList(v), Right);
                else
                    return new TreeList(Value, Left.InsertAfter(i, v), Right);
            }

            else if (RightCount == 0)
                throw new IndexOutOfRangeException();

            else
                return new TreeList(Value, Left, Right.InsertBefore(i - LeftCount - ThisCount, v));
        }

        public GTList InsertAfter(int i, Value v)
        {
            if (i < LeftCount)
                return new TreeList(Value, Left.InsertAfter(i, v), Right);

            else if (i == LeftCount && ThisCount != 0)
            {
                if (Right == null)
                    return new TreeList(Value, Left, new TreeList(v));
                else
                    return new TreeList(Value, Left, Right.InsertBefore(i, v));
            }

            else if (RightCount == 0)
                throw new IndexOutOfRangeException();

            else
                return new TreeList(Value, Left, Right.InsertAfter(i - LeftCount - ThisCount, v));
        }
        #endregion

        #region Enumeration
        public IEnumerable<Value> Enumerate()
        {
            if (Left != null)
                foreach (var v in Left.Enumerate())
                    yield return v;

            if (Value != null)
                yield return Value;

            if (Right != null)
                foreach (var v in Right.Enumerate())
                    yield return v;
        }

        public Value this[int i]
        {
            get
            {
                if (i < LeftCount)
                    return Left[i];

                else if (i == LeftCount && ThisCount != 0)
                    return Value;

                else if (RightCount == 0)
                    throw new IndexOutOfRangeException();

                else
                    return Right[i - LeftCount - ThisCount];
            }
        }
        #endregion

        public override GTType Type
        {
            get { return GTType.List; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("[");

            foreach (var v in this.Enumerate())
                sb.AppendFormat("{0} ", v.ToString());

            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            return sb.ToString();
        }
    }

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
