using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
    public static partial class Functions
    {
        public static GTVoid print(Value v)
        {
            if (v.Type == GTType.String)
                Console.WriteLine(((GTString)v).Value);
            else
                Console.WriteLine(v);

            return GTVoid.Void;
        }

        public static GTString readln()
        {
            return new GTString(Console.ReadLine());
        }
    }
}
