using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sinverse
{
    [CreateAssetMenu(fileName = "SinverseConstructionScriptable", menuName = "Scriptable Objects/SinverseConstructionScriptable", order = 1)]
    public class SinverseConstructionScriptable : ScriptableObject
    {
        public List<District> districts;
    }

    [Serializable]
    public class District
    {
        public string districtName;
        public List<SinglePlot> plots;
    }

    [Serializable]
    public class SinglePlot
    {
        public int plotId;
        public string userId;
        public string plotCoordinate;
        public string plotConstructionData;
    }
}
