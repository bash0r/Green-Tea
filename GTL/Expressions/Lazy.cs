
namespace GreenTea
{
    public class Lazy : IExpression
    {
        public IExpression Body { get; private set; }

        public Lazy(IExpression body)
        {
            this.Body = body;
        }

        public Value Evaluate(Scope scope)
        {
            return new GTLazy(Body, scope.Close());
        }

        public override string ToString()
        {
            return '&' + Body.ToString();
        }
    }
}
