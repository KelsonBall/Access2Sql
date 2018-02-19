using SqlTokenizer;
using System;

namespace KelsonBall.Access2Sql
{
    public class LinkedToken
    {
        public LinkedToken Root()
        {
            var root = this;
            var result = this;
            while ((root = root.Previous) != null)
                result = root;
            return result;
        }

        public LinkedToken Previous;
        public LinkedToken Next;
        public Token Value;

        private enum EscapeState
        {
            Valid,
            Escaped,
        }

        public LinkedToken NextNonWhitespace()
        {
            var token = this;
            while ((token = token.Next) != null)
                if (token.Value.Type != TokenType.Whitespace)
                    return token;
            return null;
        }

        public bool NextExistsAnd(Func<LinkedToken, bool> predicate, bool coalesceTo = false)
        {
            if (Next == null)
                return coalesceTo;
            else
                return predicate(Next);
        }

        public void MutabelyEnumerateNonEscaped(Action<LinkedToken> tokenAction, Func<bool> @while = null)
        {
            @while = @while ?? (() => true);
            var token = this;
            EscapeState state = EscapeState.Valid;
            int escapedStackHeight = 0;

            while ((token = token.Next) != null && @while())
            {
                switch (state)
                {
                    case EscapeState.Valid:
                        if (token.Value.Equals(TokenType.Keyword, "["))
                        {
                            escapedStackHeight++;
                            state = EscapeState.Escaped;
                        }
                        else
                        {
                            tokenAction(token);
                        }
                        break;
                    case EscapeState.Escaped:
                        if (token.Value.Equals(TokenType.Symbol, "["))
                            escapedStackHeight++;
                        else if (token.Value.Equals(TokenType.Symbol, "]"))
                            escapedStackHeight--;
                        if (escapedStackHeight == 0)
                            state = EscapeState.Valid;
                        break;
                    default:
                        break;
                }
            }
        }

        public void MutabelyEnumerateNonEscaped(Func<LinkedToken, LinkedToken> tokenFunc, Func<bool> @while = null)
        {
            @while = @while ?? (() => true);
            var token = this;
            LinkedToken next = null;
            EscapeState state = EscapeState.Valid;
            int escapedStackHeight = 0;

            while ((token = next == null ? token.Next : next) != null && @while())
            {
                switch (state)
                {
                    case EscapeState.Valid:
                        if (token.Value.Equals(TokenType.Keyword, "["))
                        {
                            escapedStackHeight++;
                            state = EscapeState.Escaped;
                        }
                        else
                        {
                            next = tokenFunc(token);
                        }
                        break;
                    case EscapeState.Escaped:
                        if (token.Value.Equals(TokenType.Symbol, "["))
                            escapedStackHeight++;
                        else if (token.Value.Equals(TokenType.Symbol, "]"))
                            escapedStackHeight--;
                        if (escapedStackHeight == 0)
                            state = EscapeState.Valid;
                        break;
                    default:
                        break;
                }
            }
        }

        #region List Operations

        public LinkedToken Append(LinkedToken token)
        {
            token.Previous = this;
            token.Next = Next;

            Next = token;

            return token;
        }

        public LinkedToken Append(Token token) => Append(new LinkedToken { Value = token });

        public LinkedToken Append(TokenType type, string source) => Append(new Token { Type = type, Source = source });

        public void AppendRange(LinkedToken from, LinkedToken to)
        {
            from.Previous = this;
            to.Next = Next;

            Next.Previous = to;
            Next = from;
        }

        public LinkedToken Prepend(LinkedToken token)
        {
            token.Next = this;
            token.Previous = Previous;

            Previous = token;

            return token;
        }

        public LinkedToken Prepend(Token token) => Prepend(new LinkedToken { Value = token });

        public LinkedToken Prepend(TokenType type, string source) => Prepend(new Token { Type = type, Source = source });

        public void PrependRange(LinkedToken from, LinkedToken to)
        {
            from.Previous = Previous;
            to.Next = this;

            Previous.Next = from;
            Previous = to;

        }

        public static void Remove(LinkedToken token)
        {
            token.Previous.Next = token.Next;
            token.Next.Previous = token.Previous;
            token.Next = null;
            token.Previous = null;
        }

        public static void RemoveRange(LinkedToken from, LinkedToken to)
        {
            from.Previous.Next = to.Next;
            to.Next.Previous = from.Previous;
            from.Previous = null;
            to.Next = null;
        }

        #endregion

        #region Ctors

        public LinkedToken()
        {

        }

        public LinkedToken(Token value)
        {
            Value = value;
        }

        #endregion

        public override string ToString() => " " + (Previous == null ? "[" : "(") + " \"" + Value.Source  + "\" " + (Next == null ? "]" : ")") + " ";
    }
}
