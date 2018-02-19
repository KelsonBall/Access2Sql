using SqlTokenizer;
using System;

namespace KelsonBall.Access2Sql.Heuristics
{
    public class NullHeuristic : Heuristic
    {
        private static readonly string Keyword_Coalesce = "COALESCE";
        private static readonly string Keyword_NZ = "NZ";
        private static readonly string Keyword_IsNull = "ISNULL";

        protected override void MutationLogic(LinkedToken tokens, bool verbose, Action<string> log)
        {
            if (verbose)
                log("Starting ISNULL Heuristic...");

            if (verbose)
                log("   Checking for NZ(Value, ValueToReturnIfNull)");
            // Rule #1, NZ/COALESCE
            tokens.MutabelyEnumerateNonEscaped(token =>
            {
                if (token.Value.Equals(TokenType.Word, Keyword_NZ) && token.NextExistsAnd(next => next.Value.Equals(TokenType.Symbol, "(")))
                    token.Value.Source = Keyword_NZ;
            });

            if (verbose)
                log("   Checking for ISNULL(Value)");

            // Rule #2, ISNULL(Value)
            tokens.MutabelyEnumerateNonEscaped(
                token =>
                {
                    if (token.Value.Equals(TokenType.Word, Keyword_IsNull) && token.NextExistsAnd(next => next.Value.Equals(TokenType.Symbol, "(")))
                    {
                        int parenStack = 1;
                        var valueStart = token.Next /* <- open paren */ .Next /* <- first token of value parameter */;
                        LinkedToken valueEnd = null;

                        valueStart.MutabelyEnumerateNonEscaped(
                            valueToken =>
                            {
                                if (valueToken.Value.Equals(TokenType.Symbol, "("))
                                    parenStack++;
                                else if (valueToken.Value.Equals(TokenType.Symbol, ")"))
                                    parenStack--;
                                if (parenStack == 0)
                                    valueEnd = valueToken.Previous;
                            },
                            @while: () => valueEnd == null
                        );

                        // State
                        // [Previous] [ISNULL] [(] [valueStart] ... [valueEnd] [)] [Next]

                        // remove [valueStart] ... [valueEnd]
                        LinkedToken.RemoveRange(valueStart, valueEnd);
                        // remove parens
                        LinkedToken.RemoveRange(token.Next, token.Next.Next);
                        valueEnd = valueEnd.Append(TokenType.Whitespace, " ");
                        token.PrependRange(valueStart, valueEnd);

                        // State
                        // [Previous] [valueStart] ... [valueEnd] [ ] [ISNULL] [Next]
                        token.Value.Source = "IS";
                        token = token.Append(TokenType.Whitespace, " ");
                        token = token.Append(TokenType.Keyword, "NULL");

                        // State
                        // [Previous] [valueStart] ... [valueEnd] [IS] [NULL] [Next]

                        // since we changed stuff, explicitly state next token in enumeration
                        return token.Next;
                    }
                    // if nothing changed, just continue to next token
                    return null;
                }
            );

            if (verbose)
                log("   NULL Heuristic complete");
        }
    }
}
