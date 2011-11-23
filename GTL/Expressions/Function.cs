using System;
using System.Collections.Generic;
using System.Linq;
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
            return new Function(Body, scope.Close(), Parameters);
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

            Function f = (Function)v;

            // Check the parameter count
            if (Parameters.Count > f.Parameters.Count)
                throw new InvalidOperationException("Too many arguments in call to function: " + Function.ToString());

            // Create evaluation context
            Scope s = new Scope(f.Container, scope.Namespace);

            for (int i = 0; i < Parameters.Count; i++)
                s.Add(f.Parameters[i], Parameters[i].Evaluate(scope));

            // All parameters filled
            if (Parameters.Count == f.Parameters.Count)
                return f.Body.Evaluate(s);

            // Partial application: Create a new function
            var newparams = new List<string>();

            for (int i = f.Parameters.Count - Parameters.Count; i < f.Parameters.Count; i++)
                newparams.Add(f.Parameters[i]);

            return new Function(f.Body, s, newparams);
        }
    }
}
