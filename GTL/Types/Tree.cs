using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
    public sealed class Tree : Value
    {
        private Value Value;
        private Value Left;
        private Value Right;

        #region Constructors
        public Tree() { }

        public Tree(Value val) : this()
        {
            this.Value = val;
            this.ThisCache = 1;
        }

        public Tree(Value val, Value left, Value right)
        {
            this.Value = val;
            this.Left = left;
            this.Right = right;
        }

        public Tree(IEnumerable<Value> list) : this()
        {
            var v = this;
            
            foreach (Value ex in list)
                v = (Tree)v.Add(ex);

            // fake mutator
            this.Value = v.Value;
            this.Left = v.Left;
            this.Right = v.Right;
        }

        public Tree(params Value[] list) : this((IEnumerable<Value>)list) { }
        #endregion

        #region Caching
        private int? LeftCache;
        private int? RightCache;
        private int? ThisCache;

        public override int Count
        {
            get
            {
                return LeftCount + ThisCount + RightCount;
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
        public override Value Add(Value v)
        {
            if (Right == null)
                if (Value == null)
                    return new Tree(v, Left, Right);
                else
                    return new Tree(Value, Left, new Tree(v));
            else
                return new Tree(Value, Left, Right.Add(v));
        }

        public override Value Set(int i, Value v)
        {
            if (i < LeftCount)
                return new Tree(Value, Left.Set(i, v), Right);

            else if (i == LeftCount && ThisCount != 0)
                return new Tree(v, Left, Right);

            else if (RightCount == 0)
                throw new IndexOutOfRangeException();

            else
                return new Tree(Value, Left, Right.Set(i - LeftCount - ThisCount, v));
        }

        public override Value InsertBefore(int i, Value v)
        {
            if (i < LeftCount)
                return new Tree(Value, Left.InsertBefore(i, v), Right);

            else if (i == LeftCount && ThisCount != 0)
            {
                if (Left == null)
                    return new Tree(Value, new Tree(v), Right);
                else
                    return new Tree(Value, Left.InsertAfter(i, v), Right);
            }

            else if (RightCount == 0)
                throw new IndexOutOfRangeException();

            else
                return new Tree(Value, Left, Right.InsertBefore(i - LeftCount - ThisCount, v));
        }

        public override Value InsertAfter(int i, Value v)
        {
            if (i < LeftCount)
                return new Tree(Value, Left.InsertAfter(i, v), Right);

            else if (i == LeftCount && ThisCount != 0)
            {
                if (Right == null)
                    return new Tree(Value, Left, new Tree(v));
                else
                    return new Tree(Value, Left, Right.InsertBefore(i, v));
            }

            else if (RightCount == 0)
                throw new IndexOutOfRangeException();

            else
                return new Tree(Value, Left, Right.InsertAfter(i - LeftCount - ThisCount, v));
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
