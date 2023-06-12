using UnityEngine.Events;

namespace SinSity.Core {
    public interface IModernizationObserver {
        event UnityAction<object, int> OnScoresReceivedEvent;
        void Subscribe();
        void Unsubscribe();
    }
}