using IdleClicker;
using SinSity.Core;
using UnityEngine;

namespace Tests.Scripts {
    public class LoaderForTests {
        public static MockedGameManager LoadGameManager() {
            var mockedGameManagerPath = "[MOCKED GAME MANAGER]";
            var mockedGameManagerPrefab = Resources.Load<MockedGameManager>(mockedGameManagerPath);
            return Object.Instantiate(mockedGameManagerPrefab);
        }
        
        public static IdleObject[] LoadIdleObject() {
            var idleObjects = Resources.LoadAll<IdleObject>("Prefabs/IdleObjects");
            var idleObjectsAmount = idleObjects.Length;
            var idleObjectsInstances = new IdleObject[idleObjectsAmount];
            for (var i = 0; i < idleObjectsAmount; i++) {
                idleObjectsInstances[i] =  Object.Instantiate(idleObjects[i]);
            }
            return idleObjectsInstances;
        }
        
        public static CameraController LoadCameraController() {
            var cameraSystemPath = "CameraSystem";
            var cameraSystemPrefab = Resources.Load<CameraController>(cameraSystemPath);
            return Object.Instantiate(cameraSystemPrefab);
        }
    }
}