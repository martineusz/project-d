using Pathfinding;
using UnityEngine;

namespace Environment.Gates
{
    public class SimpleGate : MonoBehaviour
    {
        private Bounds _bounds;

        private void Awake()
        {
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                _bounds = col.bounds;
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OpenGate()
        {
            GraphUpdateObject guo = new GraphUpdateObject(_bounds)
            {
                modifyWalkability = true,
                setWalkability = true
            };

            AstarPath.active.UpdateGraphs(guo);

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GraphUpdateObject guo = new GraphUpdateObject(_bounds)
            {
                modifyWalkability = true,
                setWalkability = false
            };

            AstarPath.active.UpdateGraphs(guo);
        }
    }
}