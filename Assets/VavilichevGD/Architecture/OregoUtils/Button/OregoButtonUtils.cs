using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Orego.Util
{
    public static class OregoButtonUtils
    {
        public static void AddListener(this Button button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }

        public static void RemoveListeners(this Button button)
        {
            button.onClick.RemoveAllListeners();
        }

        public static void RemoveListener(this Button button, UnityAction action) {
            button.onClick.RemoveListener(action);
        }
    }
}