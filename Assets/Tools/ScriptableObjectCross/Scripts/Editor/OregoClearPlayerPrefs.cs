#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Orego.Editor
{
    public static class OregoClearPlayerPrefs
    {
        [MenuItem("Orego/ClearPlayerPrefs")]
        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
#endif