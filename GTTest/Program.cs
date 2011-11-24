using System;
using GreenTea;

namespace GTTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.AddOperator(1, "+", (l, r) => new AddOperator(l, r));

            var v = Parser.ParseString(@"

var foo = [[1, 2], [3, 4], &[5, 6]]

var expr = &from x in foo
            from y in foo
            where true
            select x

var bar = func(x) x

from n in expr
select bar(n)

1+1

");

            Console.WriteLine(v);
            Console.WriteLine();
            Console.WriteLine(v.Evaluate(new Scope(null, null)));

            Console.ReadKey(true);
        }
    }
}
