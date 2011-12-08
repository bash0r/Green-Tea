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
            from exp in SimpleExpression
            select exp,

        SetOpt =
            Set.XOr(Parse.Return(GTVoid.Void)),

        VariableInit =
            from word in Parse.String("var")
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from id in ParserPre.Identifier
            from body in SetOpt
            select new Declaration(id, body, ScopeMode.Local),

        VariablePublic =
            from word in Parse.String("public")
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from id in ParserPre.Identifier
            from body in SetOpt
            select new Declaration(id, body, ScopeMode.Public),

        VariablePrivate =
            from word in Parse.String("private")
            from s1 in Parse.WhiteSpace.AtLeastOnce()
            from id in ParserPre.Identifier
            from body in SetOpt
            select new Declaration(id, body, ScopeMode.Private),

        VariableSet =
            from id in ParserPre.Identifier
            from body in Set
            select new Assignment(id, body),

        VariableExp = VariableInit.Or(VariablePublic).Or(VariablePrivate).Or(VariableSet).Or(ParserPre.Variable);
    }
}
