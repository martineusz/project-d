using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "newBuildable", menuName = "Items/Buildings", order = 0)]
    public class BuildableData : ItemData
    {
        public GameObject prefabToSpawn;
        public Quaternion spawnRotation = Quaternion.identity;
        public override void Use()
        {
            if (prefabToSpawn != null)
            {
                Vector3 finalPosition = spawnPosition;
                Object.Instantiate(prefabToSpawn, finalPosition, spawnRotation);
            }
        }
    }
}