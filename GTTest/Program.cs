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
    1+2+3<7
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
