using System;

namespace KelsonBall.Access2Sql.Heuristics
{
    public class DatePartHeuristic : Heuristic
    {
        protected override void MutationLogic(LinkedToken tokens, bool verbose, Action<string> log)
        {
            if (verbose)
                log("Starting DATEPART Heuristic...");
        }
    }
}
