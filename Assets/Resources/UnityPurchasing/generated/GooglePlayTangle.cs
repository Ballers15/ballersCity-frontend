#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("CwIKMhHv97/7UPp1dZBQ2YG1Gj7atWwG1pgbt7lpb5SQwubFkVCqS3rApq/tCrCuLEhgBN54RFlkGMsbpIdui6ZSOcv+uNwvzZNKTHFMib6CMLOQgr+0u5g0+jRFv7Ozs7eysZrhGtthAERIiTG9MN1aCSUh4v4LNVM8cHcZzImh1PHQv4tjk2K1Kaz/HUF9VTAqAVTFynEdnKIF4STBY170wdVSjdRYxkr0V4CS0uabMLiAhd5G579hiEhMO2oJmSalUOgsDcBiBlINTrVqwMMujyKYXj2wZlMPpV/5CXi7DU/IIhdbCJL2uswrMIkFtyGJYTHSrL0yWVQuWatxzo1fQ1Aws72ygjCzuLAws7OyBFFuC/ro1mi4DNRZjeBrz7Cxs7Kz");
        private static int[] order = new int[] { 12,3,8,7,12,9,12,12,9,9,11,11,13,13,14 };
        private static int key = 178;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
