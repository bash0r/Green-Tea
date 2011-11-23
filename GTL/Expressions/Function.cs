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
}
