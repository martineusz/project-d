using UnityEngine;

namespace Items
{
    public class SpawnableData : ItemData
    {
        public GameObject prefabToSpawn;
        public Transform spawnPoint;
        
        public override void Use()
        {
            if (prefabToSpawn != null && spawnPoint != null)
            {
                Object.Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}