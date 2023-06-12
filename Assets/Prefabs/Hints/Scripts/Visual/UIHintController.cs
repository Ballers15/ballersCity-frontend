using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.UI;

namespace SinSity.Core
{
    public sealed class UIHintController : UIWidget<UIHintControllerProperties>
    {
        private Dictionary<Type, UIHintInpector> uiHintInpectorMap;

        private Transform myTransform;

        protected override void Awake()
        {
            base.Awake();
            this.myTransform = this.transform;
            this.uiHintInpectorMap = this.properties.uiHintInpectorPrefs.ToDictionary(
                it => it.GetType(),
                it => Instantiate(it, this.myTransform)
            );
        }

        protected override void OnGameInitialized()
        {
            base.OnGameInitialized();
            foreach (var uiHintInspector in this.uiHintInpectorMap.Values)
            {
                uiHintInspector.OnInitialized();
            }
        }
    }
}