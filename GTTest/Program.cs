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
    add(5 + 4 * 2, 2/7.0)

    var foo = 9 ~<< [3, 1]
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
