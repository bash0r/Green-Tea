using System;
using GreenTea;

namespace GTTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var v = Parser.ParseString(@"

case of true 1
case of true 2

");

            Console.WriteLine(v);
            Console.WriteLine();
            Console.WriteLine(v.Evaluate(new Scope(null, null)));

            Console.ReadKey(true);
        }
    }
}
