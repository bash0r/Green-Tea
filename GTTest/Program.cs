using System;
using GreenTea;

namespace GTTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var v = Parser.ParseString("::func(x, y) x(5)(3)").Evaluate(new Scope(null, null));

            Console.ReadKey(true);
        }
    }
}
