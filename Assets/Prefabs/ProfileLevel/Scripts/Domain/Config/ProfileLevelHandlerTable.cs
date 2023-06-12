using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "ProfileLevelHandlerTable",
        menuName = "Domain/ProfileLevel/ ProfileLevelHandlerTable"
    )]
    public sealed class ProfileLevelHandlerTable : ScriptableObject
    {
        [SerializeField]
        private ScriptableProfileLevelHandler[] levelHandlers;
        
        public Dictionary<int, IProfileLevelHandler> LoadMap()
        {
            var levelHandlerMap = new Dictionary<int, IProfileLevelHandler>();
            var levelHandlersLength = this.levelHandlers.Length;
            for (var i = 0; i < levelHandlersLength; i++)
            {
                var asset = this.levelHandlers[i];
                var levelHandler = Instantiate(asset);
                var reachLevel = i + 1;
                levelHandler.reachLevel = reachLevel;
                levelHandlerMap[reachLevel] = levelHandler;
            }

            return levelHandlerMap;
        }
    }
}