namespace DevToDev
{
    public static class CustomEventParamsExtensions
    {
        public static CustomEventParams NewParam(this CustomEventParams customEventParams, string key, string value)
        {
            customEventParams.AddParam(key, value);
            return customEventParams;
        }

        public static CustomEventParams NewParam(this CustomEventParams customEventParams, string key, double value)
        {
            customEventParams.AddParam(key, value);
            return customEventParams;
        }

        public static CustomEventParams NewParam(this CustomEventParams customEventParams, string key, long value)
        {
            customEventParams.AddParam(key, value);
            return customEventParams;
        }

        public static CustomEventParams NewParam(this CustomEventParams customEventParams, string key, int value)
        {
            customEventParams.AddParam(key, value);
            return customEventParams;
        }
        
        public static CustomEventParams NewParam(this CustomEventParams customEventParams, string key, bool value)
        {
            customEventParams.AddParam(key, value ? 1 : 0);
            return customEventParams;
        }
    }
}