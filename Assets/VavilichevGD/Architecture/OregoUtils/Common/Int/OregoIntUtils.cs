using System;

namespace Orego.Util
{
    public static class OregoIntUtils
    {
        public static bool IsPositive(this int value)
        {
            return value > 0;
        }

        public static bool IsNegative(this int value)
        {
            return value < 0;
        }

        public static int Abs(this int value)
        {
            return Math.Abs(value);
        }

        public static void Times(this int count, Action action)
        {
            for (var i = 0; i < count; i++)
            {
                action.Invoke();
            }
        }

        public static int Random(int min, int max)
        {
            return new Random().Next(min, max);
        }
        
        public static int RandomSign()
        {
            var random = new Random();
            var value = random.Next(0, 2);
            if (value == 1)
            {
                return 1;
            }

            return -1;
        }
    }
}