using UnityEngine;

[CreateAssetMenu(fileName = "SwipeTrackerProperties", menuName = "SwipeTracker/SwipeTrackerProperties")]
public class SwipeTrackerProperties : ScriptableObject
{
    #region CONSTANTS

    public const string PATH = "SwipeTrackerProperties";

    #endregion
    
    [SerializeField] private bool m_detectSwipeOnlyAfterRelease = true;
    [SerializeField] private float m_minDistanceForSwipe = 0.15f;
    [SerializeField] private int m_maxTimeForSwipeMS = 250;
    [SerializeField] private float m_maxHorizontalTolerance = 0.05f;
    
    public bool detectSwipeOnlyAfterRelease => this.m_detectSwipeOnlyAfterRelease;
    public float minDistanceForSwipe => this.m_minDistanceForSwipe;
    public int maxTimeForSwipeMS => this.m_maxTimeForSwipeMS;
    public float maxHorizontalTolerance => this.m_maxHorizontalTolerance;
}
