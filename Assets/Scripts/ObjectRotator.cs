using UnityEngine;

namespace SinSity {
    public class ObjectRotator : MonoBehaviour {
        
        [SerializeField] private float speedX;
        [SerializeField] private float speedY;
        [SerializeField] private float speedZ;

        private Transform myTransform;


        private void Start() {
            myTransform = transform;
        }

        private void Update() {
            float deltaTime = Time.deltaTime;
            float rotX = speedX * deltaTime;
            float rotY = speedY * deltaTime;
            float rotZ = speedZ * deltaTime;
            
            myTransform.Rotate(rotX, rotY, rotZ);
        }
    }
}