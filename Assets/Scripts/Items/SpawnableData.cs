using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "newSpawnable", menuName = "Items/Spawnables", order = 0)]
    public class SpawnableData : ItemData
    {
        public GameObject prefabToSpawn;
        public Vector3 spawnPosition;
        public Quaternion spawnRotation = Quaternion.identity;
        public float spawnRadius = 0f;

        public override void Use()
        {
            if (prefabToSpawn != null)
            {
                Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                Vector3 finalPosition = spawnPosition + randomOffset;
                Object.Instantiate(prefabToSpawn, finalPosition, spawnRotation);
            }
        }
    }
}