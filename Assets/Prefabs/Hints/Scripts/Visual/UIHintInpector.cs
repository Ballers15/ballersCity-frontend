using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Core
{
    public abstract class UIHintInpector : MonoBehaviour
    {
        protected HintSystemInteractor hintSystemInteractor { get; private set; }

        public virtual void OnInitialized()
        {
            this.hintSystemInteractor = Game.GetInteractor<HintSystemInteractor>();
        }
    }
}