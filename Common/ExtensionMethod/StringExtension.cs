using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.ExtensionMethod
{
    public static class StringExtensions
    {
        public static string RemoveLines(this string text, int linesCount)
        {
            var lines = Regex.Split(text, "\r\n|\r|\n").Skip(linesCount);
            return string.Join(Environment.NewLine, lines.ToArray());
        }

        public static int NthIndexOf(this string target, string value, int n)
        {
            Match m = Regex.Match(target, "((" + Regex.Escape(value) + ").*?){" + n + "}");

            if (m.Success)
                return m.Groups[2].Captures[n - 1].Index;
            else
                return -1;
        }

        public static string ListToString(this List<string> list, string seperator = null)
        {

            if (seperator == null) seperator = Environment.NewLine;
            return string.Join(seperator, list);
        }
    }
}
