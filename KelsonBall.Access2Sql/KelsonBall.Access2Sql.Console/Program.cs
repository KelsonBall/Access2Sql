using System;
using static System.Console;

namespace KelsonBall.Access2Sql.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            HelpText();
            string line;
            string sql = "";
            while (true)
            {
                Write("> ");
                line = ReadLine();
                if (line == ".exit()")
                    return;
                else if (line == ".convert()")
                {
                    WriteLine();
                    WriteLine("input query: ");
                    WriteLine(sql);
                    WriteLine();
                    string output = sql.ToTSql();
                    WriteLine();
                    WriteLine("output query: ");
                    WriteLine(output);
                    WriteLine();
                    sql = "";
                }
                else
                    sql += line + Environment.NewLine;
            }
        }

        static void HelpText()
        {
            WriteLine("-- Access Sql to T-Sql Conversion Tool");
            WriteLine("Authored by Kelson Ball");
            WriteLine("MIT Licensed, 2018");
            WriteLine("https://github.com/KelsonBall/Access2Sql");
            WriteLine();
            WriteLine("USAGE:");
            WriteLine("Enter lines of access sql to add them to your query");
            WriteLine("Enter    .convert()      to convert your query to TSql");
            WriteLine("Enter    .exit()         to exit the tool");
            WriteLine();
        }
    }
}
