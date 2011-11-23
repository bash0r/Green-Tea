using System;
using System.Collections.Generic;

namespace GreenTea
{
    public class Select : IExpression
    {
        private SelectExpr root;

        internal Select(IEnumerable<SelectExpr> expr)
        {
            SelectExpr current = null;

            foreach (var s in expr)
                if (current == null)
                {
                    root = s;
                    current = s;
                }
                else
                {
                    current.child = s;
                    current = s;
                }
        }

        public Value Evaluate(Scope scope)
        {
            return root.Evaluate(scope);
        }

        public override string ToString()
        {
            return root.ToString();
        }
    }

    internal abstract class SelectExpr : IExpression
    {
        internal SelectExpr child;
        internal IExpression body;

        public abstract Value Evaluate(Scope scope);
    }

    internal class FromIn : SelectExpr
    {
        internal string name;

        internal FromIn(string name, IExpression body)
        {
            this.name = name;
            this.body = body;
        }

        public override Value Evaluate(Scope scope)
        {
            Value ret = new GTTree();

            foreach (var v in body.Evaluate(scope).Enumerate())
            {
                Scope s = new Scope(scope);
                s.Add(name, v);

                ret = ret.AddRange(child.Evaluate(s));
            }

            return ret;
        }

        public override string ToString()
        {
            return String.Format("from {0} in {1}\n{2}", name, body, child);
        }
    }

    internal class Where : SelectExpr
    {
        internal Where(IExpression body)
        {
            this.body = body;
        }

        public override Value Evaluate(Scope scope)
        {
            if (body.Evaluate(scope).IsTrue())
                return child.Evaluate(scope);
            else
                return new GTTree();
        }

        public override string ToString()
        {
            return String.Format("where {0}\n{1}", body, child);
        }
    }

    internal class SelectBody : SelectExpr
    {
        internal SelectBody(IExpression body)
        {
            this.body = body;
        }

        public override Value Evaluate(Scope scope)
        {
            return new GTTree(body.Evaluate(scope));
        }

        public override string ToString()
        {
            return String.Format("select {0}", body);
        }
    }
}
