using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NTK_Support
{
    public class AlphanumericComparer : IComparer<string>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        static extern int StrCmpLogicalW(string s1, string s2);

        public int Compare(string x, string y) => StrCmpLogicalW(x, y);
    }
}
