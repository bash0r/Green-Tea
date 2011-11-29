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
    private Point = func(x,y) scope {
        public X = x
        public Y = y

        public Sum = X+Y

        public SetX = func(x) Point(x, Y)
    }

    var A = Point(3, 5)
    var B = Point(8, 7)

    var List = [A, B]
    
    List.Sum

    var ListTwo = :List.SetX(1)

    ListTwo.Sum
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
