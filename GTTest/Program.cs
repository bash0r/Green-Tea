using System;
using System.Collections.Generic;
using GreenTea;

namespace GTTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var v = Parser.ParseString(@"{
    var A = [1, 2, 3]
    var B = [4, 5, 6]

    (A+B)
}");

                Console.WriteLine(v);
                Console.WriteLine();

                Module m = new Module("test", new List<string>(), v);
                var res = v.Evaluate(m.Root);
                Console.WriteLine(res);

                // try self compiling
                try
                {
                    Parser.ParseString(v.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed self-evaluation:\n{0}", e);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Failed evaluation:\n{0}", e);
            }

            Console.ReadKey(true);
        }
    }
}
