using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ExamPaperParser.Helpers
{
    public static class StringExtensions
    {
        public static string ReplaceBetween(this string str, string left, string right, string replacement = " ")
        {
            var r = new Regex($"({left}.*?{right})");
            return r.Replace(str, replacement);
        }
    }
}
