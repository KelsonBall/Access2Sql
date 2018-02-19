using SqlTokenizer;
using System;
using System.Linq;
using System.Text;

namespace KelsonBall.Access2Sql
{
    public static class Access2SqlConverter
    {
        private static readonly Heuristic[] Heuristics =
            typeof(Heuristic).Assembly
                             .GetTypes()
                             .Where(t => t.IsSubclassOf(typeof(Heuristic)))
                             .Select(h => (Heuristic)h.GetConstructor(new Type[0]).Invoke(new object[0]))
                             .ToArray();

        public static string ToTSql(this string accessQuerySql, bool verbose = false)
        {
            var tokens =
                new Tokenizer(accessQuerySql)
                    .GetTokens()
                    .Aggregate(
                        new LinkedToken(new Token { Type = TokenType.StartOfQuery, Source = "" }),
                        (linked, value) =>
                        {
                            linked.Next = new LinkedToken
                            {
                                Previous = linked,
                                Value = value
                            };
                            return linked.Next;
                        });
            tokens.Next = new LinkedToken(new Token { Type = TokenType.EndOfQuery, Source = "" });
            tokens.Next.Previous = tokens;
            tokens = tokens.Root();

            foreach (var heuristic in Heuristics)
                heuristic.Mutate(tokens, verbose);

            var reconstruction = new StringBuilder();
            LinkedToken token = tokens.Root();
            while ((token = token.Next) != null)
                reconstruction.Append(token.Value.Source);

            return reconstruction.ToString();
        }
    }
}
