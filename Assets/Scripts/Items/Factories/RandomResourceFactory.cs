using System.Collections.Generic;
using UnityEngine;

namespace Items.Factories
{
    public class RandomResourceFactory : AbstractResourceFactory
    {
        public List<ResourceData> resourcePool = new List<ResourceData>();

        public override ResourceData GenerateNewResource()
        {
            if (resourcePool == null || resourcePool.Count == 0)
            {
                Debug.LogWarning("Resource pool is empty or null.");
                return null;
            }

            int randomIndex = Random.Range(0, resourcePool.Count);
            return resourcePool[randomIndex];
        }
    }
}