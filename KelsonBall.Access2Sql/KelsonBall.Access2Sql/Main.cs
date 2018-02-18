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

        public static string ToTSql(this string accessQuerySql)
        {
            var tokens =
                new Tokenizer(accessQuerySql)
                    .GetTokens()
                    .Aggregate(
                        new LinkedToken(),
                        (linked, value) =>
                        {
                            if (linked.Root == null)
                                linked.Root = linked;
                            linked.Next = new LinkedToken
                            {
                                Previous = linked,
                                Root = linked.Root,
                                Value = value
                            };
                            return linked.Next;
                        })
                    .Root;

            foreach (var heuristic in Heuristics)
                heuristic.Mutate(tokens);

            var reconstruction = new StringBuilder();
            LinkedToken token = tokens.Root;
            while ((token = token.Next) != null)
                reconstruction.Append(token.Value.Value);

            return reconstruction.ToString();
        }
    }
}
