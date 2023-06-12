using UnityEngine;

public class FXRenovationGears : MonoBehaviour
{
    [SerializeField] private Transform part1;
    [SerializeField] private float speed1;
        
    [SerializeField] private Transform part2;
    [SerializeField] private float speed2;
        
    [SerializeField] private Transform part3;
    [SerializeField] private float speed3;

    private Vector3 localEulers1;
    private Vector3 localEulers2;
    private Vector3 localEulers3;

    public void OnEnable() {
        this.localEulers1 = this.part1.localEulerAngles;
        this.localEulers2 = this.part2.localEulerAngles;
        this.localEulers3 = this.part3.localEulerAngles;
    }

    private void Update() {
        float deltaTime = Time.deltaTime;

        this.localEulers1.z += this.speed1 * deltaTime;
        this.part1.localEulerAngles = this.localEulers1;
            
        this.localEulers2.z += this.speed2 * deltaTime;
        this.part2.localEulerAngles = this.localEulers2;
            
        this.localEulers3.z += this.speed3 * deltaTime;
        this.part3.localEulerAngles = this.localEulers3;
    }
}
