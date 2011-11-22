using System;
using GreenTea;

namespace GTTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Module test = new Module();

            GTList l = new TreeList(new GTInt(5), new GTInt(3), new GTString("foobar"), new GTFloat(2.5));
            Console.WriteLine(l);
            Console.WriteLine(l.Add(new GTVoid()));

            for (int i = 0; i < l.Count; i++)
                Console.WriteLine(l[i]);

            GTList n = new ExpressionList(new TreeList(new GTString("lazy"), l, l), null);

            Console.WriteLine(n);

            for (int i = 0; i < n.Count; i++)
                Console.WriteLine(n.Set(i, new GTString("foo")));

            Console.WriteLine();

            GTList t = new TreeList(new GTInt(1), new GTInt(2), new GTInt(3), new GTInt(4));
            Console.WriteLine(t);

            for (int i = 0; i < t.Count; i++)
            {
                Console.WriteLine(t.InsertBefore(i, new GTInt(0)));
                Console.WriteLine(t.InsertAfter(i, new GTInt(0)));
            }

            new Declaration("foobar", new ExpressionList(new GTString("foo"), test.Root), ScopeMode.Export).Evaluate(test.Root);

            Console.ReadKey(true);
        }
    }
}
