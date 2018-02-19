/*  Authored by Rony Moura and modified by Kelson Ball for user with Access SQL
 *  from https://github.com/ronymmoura/sql-tokenizer
 *  MIT Licensed
 *  https://github.com/ronymmoura/sql-tokenizer/blob/master/LICENSE
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlTokenizer
{
    /// <summary>
    /// Enum that contains the types that one token can be.
    /// </summary>
    public enum TokenType
    {
        None = 0,
        StartOfQuery,
        Whitespace,
        Seperator,
        Word,
        Keyword,
        Number,
        String,
        Symbol,
        Variable,
        EndOfQuery,
    }
}