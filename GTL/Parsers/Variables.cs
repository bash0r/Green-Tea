using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace GreenTea
{
    public static partial class Parser
    {
        private static Parser<IExpression>

        Set =
            from s1 in Parse.WhiteSpace.Many()
            from eq in Parse.Char('=')
            from s2 in Parse.WhiteSpace.Many()
            from exp in Expression
            select exp,

        SetOpt =
            Set.XOr(Parse.Return(GTVoid.Void)),

        VariableInit =
            from word in Parse.String("var")
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from id in Identifier
            from body in SetOpt
            select new Declaration(id, body, ScopeMode.Local),

        VariableExport =
            from word in Parse.String("export")
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from id in Identifier
            from body in SetOpt
            select new Declaration(id, body, ScopeMode.Export),

        VariableStatic =
            from word in Parse.String("static")
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from id in Identifier
            from body in SetOpt
            select new Declaration(id, body, ScopeMode.Static),

        VariableSet =
            from id in Identifier
            from body in Set
            select new Assignment(id, body),

        VariableUsage =
            from id in Identifier
            select new Usage(id),

        VariableExp = VariableInit.Or(VariableExport).Or(VariableStatic).Or(VariableSet).Or(VariableUsage);
    }
}
