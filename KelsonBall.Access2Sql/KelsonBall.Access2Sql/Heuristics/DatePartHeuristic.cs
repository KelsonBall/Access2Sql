using System;

namespace KelsonBall.Access2Sql.Heuristics
{
    public class DatePartHeuristic : Heuristic
    {
        public override void Mutate(LinkedToken tokens)
        {
            Console.WriteLine("Doing the datepart heuristic...");
        }
    }
}
