using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
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
}
