using System;

namespace KelsonBall.Access2Sql
{
    /// <summary>
    /// Encapsulates a modification of a sequence of tokens
    /// Heuristcs based on the documentation at http://weblogs.sqlteam.com/jeffs/archive/2007/03/30/Quick-Access-JET-SQL-to-T-SQL-Cheatsheet.aspx
    /// </summary>
    public abstract class Heuristic
    {
        protected abstract void MutationLogic(LinkedToken tokens, bool verbose, Action<string> log);

        public void Mutate(LinkedToken tokens, bool verbose, Action<string> log = null)
        {
            MutationLogic(tokens, verbose, log ?? Console.WriteLine);
        }
    }
}
