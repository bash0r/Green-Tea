using System;
using System.Collections.Generic;
using System.Text;

namespace GreenTea
{
    public sealed class GTTree : Value
    {
        private Value Value;
        private Value Left;
        private Value Right;

        #region Constructors
        public GTTree() { }

        public GTTree(Value val) : this()
        {
            this.Value = val;
            this.ThisCache = 1;
        }

        public GTTree(Value val, Value left, Value right)
        {
            this.Value = val;
            this.Left = left;
            this.Right = right;
        }

        public GTTree(IEnumerable<Value> list) : this()
        {
            var v = new GTTree();
            
            foreach (Value ex in list)
                v = (GTTree)v.Add(ex);

            // fake mutator
            this.Value = v.Value;
            this.Left = v.Left;
            this.Right = v.Right;
        }

        public GTTree(params Value[] list) : this((IEnumerable<Value>)list) { }
        #endregion

        #region Caching
        private int? LeftCCache;
        private int? RightCCache;
        private int? LeftSCache;
        private int? RightSCache;
        private int? ThisCache;

        public override int Count
        {
            get
            {
                return LeftCount + ThisCount + RightCount;
            }
        }

        public override int Size
        {
            get
            {
                return LeftSize + ThisSize + RightSize;
            }
        }

        private int LeftCount
        {
            get
            {
                if (LeftCCache == null)
                    LeftCCache = (Left ?? GTVoid.Void).Count;

                return LeftCCache.Value;
            }
        }

        private int RightCount
        {
            get
            {
                if (RightCCache == null)
                    RightCCache = (Right ?? GTVoid.Void).Count;

                return RightCCache.Value;
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

        private int LeftSize
        {
            get
            {
                if (LeftSCache == null)
                    LeftSCache = (Left ?? GTVoid.Void).Size;

                return LeftSCache.Value;
            }
        }

        private int RightSize
        {
            get
            {
                if (RightSCache == null)
                    RightSCache = (Right ?? GTVoid.Void).Size;

                return RightSCache.Value;
            }
        }

        private int ThisSize
        {
            get
            {
                return ThisCount;
            }
        }
        #endregion

        #region Modification
        public override Value Add(Value v)
        {
            if (LeftSize <= RightSize)
                return new GTTree(v, this, null);
            else
                if (Right == null)
                    return new GTTree(Value, Left, new GTTree(v));
                else
                    return new GTTree(Value, Left, Right.Add(v));
        }

        public override Value AddRange(Value v)
        {
            if (Right == null)
                return new GTTree(Value, Left, v);
            else
                return new GTTree(Value, Left, Right.AddRange(v));
        }

        public override Value Set(int i, Value v)
        {
            if (i < LeftCount)
                return new GTTree(Value, Left.Set(i, v), Right);

            else if (i == LeftCount && ThisCount != 0)
                return new GTTree(v, Left, Right);

            else if (RightCount == 0)
                throw new IndexOutOfRangeException();

            else
                return new GTTree(Value, Left, Right.Set(i - LeftCount - ThisCount, v));
        }

        public override Value InsertBefore(int i, Value v)
        {
            if (i < LeftCount)
                return new GTTree(Value, Left.InsertBefore(i, v), Right);

            else if (i == LeftCount && ThisCount != 0)
            {
                if (Left == null)
                    return new GTTree(Value, new GTTree(v), Right);
                else
                    return new GTTree(Value, Left.InsertAfter(i, v), Right);
            }

            else if (RightCount == 0)
                throw new IndexOutOfRangeException();

            else
                return new GTTree(Value, Left, Right.InsertBefore(i - LeftCount - ThisCount, v));
        }

        public override Value InsertAfter(int i, Value v)
        {
            if (i < LeftCount)
                return new GTTree(Value, Left.InsertAfter(i, v), Right);

            else if (i == LeftCount && ThisCount != 0)
            {
                if (Right == null)
                    return new GTTree(Value, Left, new GTTree(v));
                else
                    return new GTTree(Value, Left, Right.InsertBefore(i, v));
            }

            else if (RightCount == 0)
                throw new IndexOutOfRangeException();

            else
                return new GTTree(Value, Left, Right.InsertAfter(i - LeftCount - ThisCount, v));
        }
        #endregion

        #region Enumeration
        public override IEnumerable<Value> Enumerate()
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

        public override Value this[int i]
        {
            get
            {
                if (i < LeftCount)
                    return Left[i];

                else if (i == LeftCount && ThisCount != 0)
                    return Value;

                else if (Right == null)
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
            StringBuilder sb = new StringBuilder();

            foreach (var v in this.Enumerate())
                sb.AppendFormat(" {0}", v.ToString());

            if (sb.Length > 0)
                sb.Remove(0, 1);

            return String.Format("[{0}]", sb);
        }
    }
}
