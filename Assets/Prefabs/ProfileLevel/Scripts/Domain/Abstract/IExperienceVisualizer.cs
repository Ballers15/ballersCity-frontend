using System;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public interface IExperienceVisualizer : IInteractor
    {
        event Action<object, Transform, ulong> OnVisualizeAddedExperienceEvent;
    }
}