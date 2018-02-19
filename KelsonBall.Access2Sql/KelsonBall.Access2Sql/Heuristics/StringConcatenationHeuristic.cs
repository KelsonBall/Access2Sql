using System;
using System.Collections.Generic;
using System.Text;

namespace KelsonBall.Access2Sql.Heuristics
{
    public class StringConcatenationHeuristic : Heuristic
    {
        protected override void MutationLogic(LinkedToken tokens, bool verbose, Action<string> log)
        {
            if (verbose)
                log("Starting STRING CONCATENATION Heuristic...");
        }
    }
}
