using System;
using GreenTea;

namespace GTTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var v = Parser.ParseString(@"

var foo = [[1, 2], [3, 4], &dfgdfgh]

var expr = &from x in foo
            from y in foo
            where false
            select x

");

            Console.WriteLine(v);
            Console.WriteLine();
            Console.WriteLine(v.Evaluate(new Scope(null, null)));

            Console.ReadKey(true);
        }
    }
}
