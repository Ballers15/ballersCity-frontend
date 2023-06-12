using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "GemBranchDataVersionUpdater",
        menuName = "Repo/GemTree/New GemBranchDataVersionUpdater"
    )]
    public sealed class GemBranchDataVersionUpdater : ScriptableObject, IVersionUpdater<GemBranchDataSet>
    {
        [SerializeField]
        private GemBranchDataSet startGemBranchDataSet;

        public bool UpdateVersion(ref GemBranchDataSet data)
        {
            if (data != null)
            {
                return false;
            }

            data = this.startGemBranchDataSet.Clone();
            return true;
        }
    }
}