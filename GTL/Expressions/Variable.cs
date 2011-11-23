using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
    public class Declaration : IExpression
    {
        public string Name { get; protected set; }
        public IExpression Body { get; protected set; }
        public ScopeMode Mode { get; protected set; }

        public Declaration(string name, IExpression body = null, ScopeMode mode = ScopeMode.Local)
        {
            this.Name = name;
            this.Body = body ?? GTVoid.Void;
            this.Mode = mode;
        }

        public Value Evaluate(Scope scope)
        {
            Value v = Body.Evaluate(scope);
            scope.Add(Name, v, Mode);

            return v;
        }

        public override string ToString()
        {
            return (Mode == ScopeMode.Local ? "var" : Mode.ToString().ToLower()) + " " + Name + (Body == null ? "" : " = " + Body.ToString());
        }
    }

    public class Assignment : IExpression
    {
        public string Name { get; protected set; }
        public IExpression Body { get; protected set; }

        public Assignment(string name, IExpression body)
        {
            this.Name = name;
            this.Body = body;
        }

        public Value Evaluate(Scope scope)
        {
            return scope.Find(Name, Body.Evaluate(scope));
        }

        public override string ToString()
        {
            return Name + " = " + Body.ToString();
        }
    }

    public class Usage : IExpression
    {
        public string Name { get; protected set; }

        public Usage(string name)
        {
            this.Name = name;
        }

        public Value Evaluate(Scope scope)
        {
            return scope.Find(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
