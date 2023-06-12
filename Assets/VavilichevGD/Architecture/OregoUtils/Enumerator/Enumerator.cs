using System;
using System.Collections;

namespace Orego.Util
{
    public sealed class Enumerator
    {
        public static IEnumerator Always(Func<IEnumerator> routineFunc)
        {
            while (true)
            {
                yield return routineFunc.Invoke();
            }
        }

        public static IEnumerator Times(int times, Func<IEnumerator> routineFunc)
        {
            for (var i = 0; i < times; i++)
            {
                yield return routineFunc.Invoke();
            }
        }
    }
}