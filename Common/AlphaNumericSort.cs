using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Common
{
    public class AlphanumericComparer : IComparer<string>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        static extern int StrCmpLogicalW(string s1, string s2);

        public int Compare(string x, string y) => StrCmpLogicalW(x, y);
    }
}
