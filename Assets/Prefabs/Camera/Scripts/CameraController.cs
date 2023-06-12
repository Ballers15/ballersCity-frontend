using System.Collections;
using SinSity.Services;
using UnityEngine;

namespace IdleClicker
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance { get; private set; }

        public delegate void CameraMovingHandler(int fromPointIndex, int toPointIndex);

        public static event CameraMovingHandler OnCameraMovingStart;
        public static event CameraMovingHandler OnCameraMovingOver;


        [SerializeField]
        private Transform cameraTransform;

        [SerializeField]
        private Transform[] points;

        [Space, SerializeField]
        private float duration = 1f;

        private Coroutine movingRoutine;
        public int indexPointCurrent { get; private set; }
        private int indexPointPrevious;

        private void Awake()
        {
            CreateSingleton();
        }

        private void CreateSingleton()
        {
            instance = this;
        }

        private void Start()
        {
            ResetCamera();
        }

        public void ResetCamera()
        {
            indexPointCurrent = 0;
            Vector3 position = points[indexPointCurrent].position;
            SetCameraPosition(position);
        }

        public void MoveUp()
        {
            if (indexPointCurrent > 0)
            {
                Stop();

                indexPointPrevious = indexPointCurrent;
                indexPointCurrent--;
                Transform newPoint = points[indexPointCurrent];
                movingRoutine = StartCoroutine("MoveToPointRoutine", newPoint);
                OnCameraMovingStart?.Invoke(indexPointPrevious, indexPointCurrent);
                CommonAnalytics.LogArrowsClicked(this.indexPointPrevious, this.indexPointCurrent);
            }
        }
#if UNITY_EDITOR
        public void MoveUpInstantly()
        {
            int currentIndex = GetCurrentPointIndex();
            if (currentIndex > 0)
            {
                currentIndex--;
                Transform newPoint = points[currentIndex];
                cameraTransform.position = newPoint.position;
            }
        }

        private int GetCurrentPointIndex()
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].position == cameraTransform.position)
                    return i;
            }

            throw new System.Exception("Cant to find position;");
        }

        public void MoveDownInstantly()
        {
            int currentIndex = GetCurrentPointIndex();
            if (currentIndex < points.Length - 1)
            {
                currentIndex++;
                Transform newPoint = points[currentIndex];
                cameraTransform.position = newPoint.position;
            }
        }
#endif

        private void Stop()
        {
            if (movingRoutine != null)
            {
                StopCoroutine(movingRoutine);
                movingRoutine = null;
                OnCameraMovingOver?.Invoke(indexPointPrevious, indexPointCurrent);
            }
        }

        private IEnumerator MoveToPointRoutine(Transform newPoint)
        {
            float timer = 0f;
            Vector3 startPosition = cameraTransform.position;
            Vector3 finishPosition = newPoint.position;
            while (timer < 1f)
            {
                timer = Mathf.Clamp01(timer + Time.fixedDeltaTime / duration);
                Vector3 newPosition = Vector3.Lerp(cameraTransform.position, finishPosition, timer);
                SetCameraPosition(newPosition);
                yield return new WaitForFixedUpdate();
            }

            Stop();
        }

        private void SetCameraPosition(Vector3 newPosition)
        {
            cameraTransform.position = newPosition;
        }

        public void MoveDown()
        {
            if (indexPointCurrent < points.Length - 1)
            {
                Stop();
                indexPointPrevious = indexPointCurrent;
                indexPointCurrent++;
                Transform newPoint = points[indexPointCurrent];
                movingRoutine = StartCoroutine("MoveToPointRoutine", newPoint);
                OnCameraMovingStart?.Invoke(indexPointPrevious, indexPointCurrent);
                CommonAnalytics.LogArrowsClicked(this.indexPointPrevious, this.indexPointCurrent);
            }
        }

        private void Reset()
        {
            if (!cameraTransform)
            {
                Camera camera = GetComponentInChildren<Camera>(true);
                if (camera)
                    cameraTransform = camera.transform;
            }
        }
    }
}