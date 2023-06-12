using System;
using System.Collections.Generic;
using System.Linq;

namespace  Orego.Util
{
    public static class OregoBooleanUtils
    {
        public static bool RandomBoolean()
        {
            var value = new Random().Next(2);
            return value == 1;
        }
        
        public static int ToInt(this bool boolean)
        {
            return boolean ? 1 : 0;
        }
    }
}