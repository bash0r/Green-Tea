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
    export inf = :func(n) n<=&this(n+1)(0)
}");

            Console.WriteLine(v);
            Console.WriteLine();

            Module m = new Module("test", new List<string>(), v);
            var res = v.Evaluate(m.Root);
            //Console.WriteLine(res);

            var x = res[5];

            Console.ReadKey(true);
        }
    }
}
