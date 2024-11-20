using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_PLUS_COMPILER.Src
{
    class Utils
    {
        public static bool IsAlpha(char c)
        {
            bool isAlpha = false;

            int cCode = (int)c;

            if (cCode >= 97 && cCode <= 122 || cCode >= 65 && cCode <= 90)
                isAlpha = true;

            return isAlpha;
        }

        public static bool IsDigit(char c)
        {
            bool isDigit = false;

            int cCode = (int)c;

            if (cCode >= 48 && cCode <= 57)
                isDigit = true;

            return isDigit;
        }
    }
}
