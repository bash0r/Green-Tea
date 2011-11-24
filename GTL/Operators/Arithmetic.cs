using System;

namespace GreenTea
{
    public class AddOperator : OpAdapter
    {
        public AddOperator(IExpression left, IExpression right) : base(left, right) { }

        protected override Func<dynamic, dynamic, dynamic> Fun
        {
            get { return (l, r) => l + r; }
        }

        public override string ToString()
        {
            return String.Format("{0} + {1}", Left, Right);
        }
    }

    public class SubOperator : OpAdapter
    {
        public SubOperator(IExpression left, IExpression right) : base(left, right) { }

        protected override Func<dynamic, dynamic, dynamic> Fun
        {
            get { return (l, r) => l - r; }
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Left, Right);
        }
    }

    public class MulOperator : OpAdapter
    {
        public MulOperator(IExpression left, IExpression right) : base(left, right) { }

        protected override Func<dynamic, dynamic, dynamic> Fun
        {
            get { return (l, r) => l * r; }
        }

        public override string ToString()
        {
            return String.Format("{0} * {1}", Left, Right);
        }
    }

    public class DivOperator : OpAdapter
    {
        public DivOperator(IExpression left, IExpression right) : base(left, right) { }

        protected override Func<dynamic, dynamic, dynamic> Fun
        {
            get { return (l, r) => l / r; }
        }

        public override string ToString()
        {
            return String.Format("{0} / {1}", Left, Right);
        }
    }

    public class ModOperator : OpAdapter
    {
        public ModOperator(IExpression left, IExpression right) : base(left, right) { }

        protected override Func<dynamic, dynamic, dynamic> Fun
        {
            get { return (l, r) => l % r; }
        }

        public override string ToString()
        {
            return String.Format("{0} % {1}", Left, Right);
        }
    }
}
