using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            Value ret = new GTTree();
            int c = v.Count;

            foreach (var res in v.Enumerate())
            {
                var r = res.Self();

                if (r.Type != GTType.Function)
                    if (c == 1)
                        throw new InvalidOperationException("Cannot call non-function type");
                    else
                        continue;

                GTFunction f = (GTFunction)res;

                // Check the parameter count
                if (Parameters.Count > f.Parameters.Count)
                    if (c == 1)
                        throw new InvalidOperationException("Too many arguments in call to function: " + Function.ToString());
                    else
                        continue;

                // Create evaluation context
                Scope s = new Scope(f.Container);

                // Add this() for recursion
                s.Add("this", f);

                for (int i = 0; i < Parameters.Count; i++)
                    s.Add(f.Parameters[i], Parameters[i].Evaluate(scope));

                // All parameters filled
                if (Parameters.Count == f.Parameters.Count)
                {
                    Value o;

                    if (f.Body == null)
                    {
                        // Invoke the method
                        var w = (WrapperFunc)f;
                        o = (Value)w.Method.Invoke(null, (from par in w.Parameters
                                                          select new Usage(par).Evaluate(s)).ToArray());
                    }
                    else
                        o = f.Body.Evaluate(s);

                    ret = ret.Add(o);
                    continue;
                }

                // No partial application in list context
                if (c > 1)
                    continue;

                // Partial application: Create a new function
                var newparams = new List<string>();

                for (int i = f.Parameters.Count - Parameters.Count; i < f.Parameters.Count; i++)
                    newparams.Add(f.Parameters[i]);

                return new GTFunction(f.Body, s, newparams);
            }

            if (ret.Count == 1)
                return ret[0]; // single values -> skip the list stuff
            else
                return ret;
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
