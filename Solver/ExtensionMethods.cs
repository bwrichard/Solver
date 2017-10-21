using System;
using System.Collections.Generic;
using System.Text;

namespace Solver
{
    public static class ExtensionMethods
    {
        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input is null");
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        public static bool ContainsOnly(this string input, char testChar)
        {
            foreach(char c in input)
            {
                if (c != testChar)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
