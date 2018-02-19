/*  Authored by Rony Moura and modified by Kelson Ball for user with Access SQL
 *  from https://github.com/ronymmoura/sql-tokenizer
 *  MIT Licensed
 *  https://github.com/ronymmoura/sql-tokenizer/blob/master/LICENSE
 */

using System.Collections.Generic;

namespace SqlTokenizer
{
    /// <summary>
    /// Class that represents a token.
    /// </summary>
    public class Token
    {
        public TokenType Type { get; set; }
        public string Source { get; set; }

        /// <summary>
        /// Compare two tokens.
        /// </summary>
        /// <param name="type">The type of the token to be compared.</param>
        /// <param name="value">The value of the token to be compared.</param>
        /// <returns></returns>
        public bool Equals(TokenType type, string value)
        {
            return Type == type && Source.ToUpperInvariant() == value.ToUpperInvariant();
        }

        public override string ToString() => Source;
    }

    /// <summary>
    /// Extension to Queue class.
    /// </summary>
    public static class QueueExtensions
    {
        /// <summary>
        /// Verify if the queue contains a specific item.
        /// </summary>
        /// <param name="tokens">The queue to be verified</param>
        /// <param name="type">The type of the token to be compared</param>
        /// <param name="value">The value of the token to be compared</param>
        /// <returns></returns>
        public static bool Contains(this Queue<Token> tokens, TokenType type, string value)
        {
            var result = false;

            var tokensArray = tokens.ToArray();

            for(int i = 0; i < tokensArray.Length; i++)
            {
                if(tokensArray[i].Type == type && tokensArray[i].Source == value)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
