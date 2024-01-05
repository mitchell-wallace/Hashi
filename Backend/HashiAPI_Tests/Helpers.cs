using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashiAPI_Tests
{
    internal class Helpers
    {
        public static string JsonCleanup(string s)
        {
            return String.Concat(s.Where(c => !Char.IsWhiteSpace(c))).ToLower();
        }
    }
}
