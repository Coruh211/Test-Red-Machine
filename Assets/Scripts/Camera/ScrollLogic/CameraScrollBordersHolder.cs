using UnityEngine;

namespace Camera.ScrollLogic
{
    public class CameraScrollBordersHolder: MonoBehaviour
    {
        public Transform TopRightBoundary => topRightBoundary;
        public Transform BottomLeftBoundary => bottomLeftBoundary;
        
        [SerializeField] private Transform topRightBoundary;
        [SerializeField] private Transform bottomLeftBoundary;
    }
}