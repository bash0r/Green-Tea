using System;
using System.Collections.Generic;
using System.Text;

namespace GreenTea
{
    public class FunctionCreation : IExpression
    {
        public IExpression Body { get; private set; }
        public IEnumerable<string> Parameters { get; private set; }

        public FunctionCreation(IExpression body, IEnumerable<string> param)
        {
            this.Body = body;
            this.Parameters = param;
        }

        public Value Evaluate(Scope scope)
        {
            return new GTFunction(Body, scope.Close(), Parameters);
        }

        public override string ToString()
        {
            StringBuilder args = new StringBuilder();

            foreach (var p in Parameters)
                args.AppendFormat(", {0}", p);

            if (args.Length == 0)
                return String.Format("func {0}", Body);

            args.Remove(0, 2);

            return String.Format("func({0}) {1}", args, Body);
        }
    }

    public class FunctionApplication : IExpression
    {
        public IExpression Function { get; private set; }
        public List<IExpression> Parameters { get; private set; }

        public FunctionApplication(IExpression fun, IEnumerable<IExpression> args)
        {
            this.Function = fun;
            this.Parameters = new List<IExpression>(args);
        }

        public Value Evaluate(Scope scope)
        {
            // Evaluate the function
            Value v = Function.Evaluate(scope);

            if (v.Type != GTType.Function)
                throw new InvalidOperationException("Cannot call non-function type"); // TODO: list calling / overloads

            GTFunction f = (GTFunction)v;

            // Check the parameter count
            if (Parameters.Count > f.Parameters.Count)
                throw new InvalidOperationException("Too many arguments in call to function: " + Function.ToString());

            // Create evaluation context
            Scope s = new Scope(f.Container, scope.Namespace);

            // Add this() for recursion
            s.Add("this", f);

            for (int i = 0; i < Parameters.Count; i++)
                s.Add(f.Parameters[i], Parameters[i].Evaluate(scope));

            // All parameters filled
            if (Parameters.Count == f.Parameters.Count)
                return f.Body.Evaluate(s);

            // Partial application: Create a new function
            var newparams = new List<string>();

            for (int i = f.Parameters.Count - Parameters.Count; i < f.Parameters.Count; i++)
                newparams.Add(f.Parameters[i]);

            return new GTFunction(f.Body, s, newparams);
        }

        public override string ToString()
        {
            StringBuilder args = new StringBuilder();

            foreach (var ex in Parameters)
                args.AppendFormat(", {0}", ex);

            if (args.Length > 0)
                args.Remove(0, 2);

            return String.Format(":{0}({1})", Function, args);
        }
    }
}
