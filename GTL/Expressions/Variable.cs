using System;
using System.Linq;

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
        public IExpression Scope { get; protected set; }
        public string Name { get; protected set; }

        public Usage(string name, IExpression scope)
        {
            this.Name = name;
            this.Scope = scope;
        }

        public Usage(string name) : this(name, null) { }

        public Value Evaluate(Scope scope)
        {
            if (Scope == null)
                return scope.Find(Name);
            else
            {
                Value v = Scope.Evaluate(scope).Self();

                if (v.Type == GTType.List)
                    return new GTTree(from s in v.Enumerate()
                                      select new Usage(Name, s).Evaluate(scope));
                else if (v.Type == GTType.Scope)
                    return ((Scope)v).Find(Name);
                else
                    throw new InvalidOperationException("Cannot dereference from non-scope value: " + v.ToString());
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
