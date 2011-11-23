using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<string>

        Namespace =
            from word in Parse.String("namespace")
            from space in Parse.WhiteSpace.AtLeastOnce()
            from name in Identifier
            select name,

        Include =
            from word in Parse.String("include")
            from space in Parse.WhiteSpace.AtLeastOnce()
            from name in Identifier
            select name;


        private static Parser<IEnumerable<string>>

        Includes =
            from first in Include.Once()
            from rest in
                (from space in Parse.WhiteSpace.AtLeastOnce()
                 from inc in Include
                 select inc).Many()
            select first.Concat(rest);


        private static Parser<Module>

        Module =
            from name in Namespace.Or(Parse.Return("main"))
            from incs in Includes.Or(Parse.Return(new List<string>()))
            from expr in Expression
            select new Module(name, incs, expr);
    }
}
