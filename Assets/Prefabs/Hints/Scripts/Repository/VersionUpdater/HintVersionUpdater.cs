using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    public sealed class HintVersionUpdater : IVersionUpdater<HintStatistics>
    {
        public bool UpdateVersion(ref HintStatistics data)
        {
            if (data == null)
            {
                data = new HintStatistics
                {
                    hintJsonKeys = new List<string>(),
                    hintJsonValues = new List<string>()
                };
                return true;
            }

            return false;
        }


//        #region Const
//
//        private const string CONFIG_ASSET_PATH = "Config/HintConfig";
//
//        #endregion
//
//        private readonly HintConfig config;
//
//        public HintVersionUpdater()
//        {
//            this.config = Resources.Load<HintConfig>(CONFIG_ASSET_PATH);
//        }
//
//        #region UpdateVersion
//
//        public bool UpdateVersion(ref HintStatistics data)
//        {
//            if (data?.hintJsonKeys == null || data?.hintJsonValues == null)
//            {
//                data = this.BuildStatistics();
//                return true;
//            }
//
//            return this.CheckVersion(ref data);
//        }
//
//        private bool CheckVersion(ref HintStatistics data)
//        {
//            var isUpdated = false;
//            var jsonKeys = data.hintJsonKeys;
//            var jsonValues = data.hintJsonValues;
//            var hintIdSet = this.config.hintIdSet;
//            foreach (var hintId in hintIdSet)
//            {
//                if (!jsonKeys.Contains(hintId))
//                {
//                    jsonKeys.Add(hintId);
//                    jsonValues.Add(null);
//                    isUpdated = true;
//                }
//            }
//
//            return isUpdated;
//        }
//
//        private HintStatistics BuildStatistics()
//        {
//            var jsonKeys = new List<string>();
//            var jsonValues = new List<string>();
//            var hintIdSet = this.config.hintIdSet;
//            foreach (var hintId in hintIdSet)
//            {
//                jsonKeys.Add(hintId);
//                jsonValues.Add(null);
//            }
//
//            return new HintStatistics
//            {
//                hintJsonKeys = jsonKeys,
//                hintJsonValues = jsonValues
//            };
//        }
//
//        #endregion
    }
}