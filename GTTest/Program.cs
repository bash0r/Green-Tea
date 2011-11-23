using System;
using GreenTea;

namespace GTTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var v = Parser.ParseString(@"

var foo = 5
var bar = 3

var test = func(x, y) x

if (true)
    test(foo, bar)
else
    test(bar, foo)

");//.Evaluate(new Scope(null, null));

            Console.WriteLine(v);
            Console.ReadKey(true);
        }
    }
}
