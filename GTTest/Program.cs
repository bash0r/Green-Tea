using System;
using System.Collections.Generic;
using GreenTea;

namespace GTTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var v = Parser.ParseString(@"{
    var test = [
        func(x) x,
        func(x,y) x+y
    ]

    test(2.5)+test(1,3)
}");

            Console.WriteLine(v);
            Console.WriteLine();

            Module m = new Module("test", new List<string>(), v);
            var res = v.Evaluate(m.Root);
            Console.WriteLine(res);

            Console.ReadKey(true);
        }
    }
}
