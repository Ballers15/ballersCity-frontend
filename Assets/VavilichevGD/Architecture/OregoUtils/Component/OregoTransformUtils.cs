using UnityEngine;

namespace Orego.Util
{
    public static class OregoTransformUtils
    {
        public static void AddEulerRotation(this Component component, Vector3 offset)
        {
            var eulerAngles = component.GetEulerRotation();
            var eulerAnglesWithOffset = new Vector3(
                eulerAngles.x + offset.x,
                eulerAngles.y + offset.y,
                eulerAngles.z + offset.z
            );
            component.SetEulerRotation(eulerAnglesWithOffset);
        }
        
        public static void ShiftPosition(this Component component, Vector3 speed)
        {
            var position = component.GetPosition();
            var deltaTime = Time.fixedDeltaTime;
            component.SetPosition(
                new Vector3(
                    position.x + speed.x * deltaTime,
                    position.y + speed.y * deltaTime,
                    position.z + speed.z * deltaTime
                )
            );
        }
    }
}