using System;
using System.Collections.Generic;
using System.Text;

namespace GreenTea
{
    public class IfElse : IExpression
    {
        public IExpression Condition { get; private set; }
        public IExpression Body { get; private set; }
        public IExpression Else { get; private set; }

        public IfElse(IExpression cond, IExpression body, IExpression alt)
        {
            this.Condition = cond;
            this.Body = body;
            this.Else = alt;
        }

        public Value Evaluate(Scope scope)
        {
            if (Condition.Evaluate(scope).IsTrue())
                return Body.Evaluate(scope);
            else
                return Else.Evaluate(scope);
        }

        public override string ToString()
        {
            return String.Format("if {0}\n{1}\nelse\n{2}", Condition, Body, Else);
        }
    }

    public class CaseOf : IExpression
    {
        public List<Tuple<IExpression, IExpression>> Cases { get; private set; }
        public IExpression Else { get; private set; }

        public CaseOf(IEnumerable<Tuple<IExpression, IExpression>> cases, IExpression alt)
        {
            this.Cases = new List<Tuple<IExpression, IExpression>>(cases);
            this.Else = alt;
        }

        public Value Evaluate(Scope scope)
        {
            foreach (var c in Cases)
                if (c.Item1.Evaluate(scope).IsTrue())
                    return c.Item2.Evaluate(scope);

            return Else.Evaluate(scope);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("case\n");

            foreach (var c in Cases)
                sb.AppendFormat("of {0} {1}\n", c.Item1, c.Item2);

            sb.AppendFormat("else {0}", Else);

            return sb.ToString();
        }
    }
}
