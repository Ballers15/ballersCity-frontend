using System;
using System.Linq;

namespace Orego.Util
{
    public static class OregoStringUtils
    {
        #region Const

        private static readonly Random RANDOM = new Random();

        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        #endregion

        public static bool IsNotEmpty(this string str)
        {
            return str?.Length > 0;
        }

        public static string Random(int length)
        {
            return new string(Enumerable.Repeat(CHARS, length)
                .Select(s => s[RANDOM.Next(s.Length)]).ToArray());
        }
    }
}