using System.Collections.Generic;
using UnityEngine;

namespace Camera.ScrollLogic
{
    public class LevelScrollBorders: MonoBehaviour
    { 
        [SerializeField] private List<Transform> borderPoints = new List<Transform>();
        
        private void OnDrawGizmos()
        {
            if (borderPoints.Count > 1)
            {
                Gizmos.color = Color.green;
                
                for (int i = 0; i < borderPoints.Count - 1; i++)
                {
                    Gizmos.DrawLine(borderPoints[i].position, borderPoints[i + 1].position);
                }
                
                Gizmos.DrawLine(borderPoints[borderPoints.Count - 1].position, borderPoints[0].position);
            }
        }
    }
}