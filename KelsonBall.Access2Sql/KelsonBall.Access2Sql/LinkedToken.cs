using SqlTokenizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace KelsonBall.Access2Sql
{
    public class LinkedToken
    {
        public LinkedToken Root;
        public LinkedToken Previous;
        public LinkedToken Next;
        public Token Value;
    }
}
