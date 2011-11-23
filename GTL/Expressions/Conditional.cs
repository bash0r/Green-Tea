using System;

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
            Value v = Condition.Evaluate(scope);

            if (v.Type == GTType.Bool)
                if (((GTBool)v).Value)
                    return Body.Evaluate(scope);
                else
                    return Else.Evaluate(scope);
            else if (v.Count == 0)
                return Else.Evaluate(scope);
            else
                throw new InvalidOperationException("Result of " + Condition.ToString() + " is not a conditional, got " + v.Type);
        }

        public override string ToString()
        {
            return String.Format("if {0}\n{1}\nelse\n{2}", Condition, Body, Else);
        }
    }
}
