using System;
using System.Collections.Generic;
using System.Text;

namespace KelsonBall.Access2Sql.Heuristics
{
    public class StringConcatenationHeuristic : Heuristic
    {
        public override void Mutate(LinkedToken tokens)
        {
            Console.WriteLine("Doing the string concatenation heuristic...");
        }
    }
}
