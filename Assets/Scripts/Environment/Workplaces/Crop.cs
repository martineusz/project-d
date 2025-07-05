using Items;
using UnityEngine;

namespace Environment.Workplaces
{
    public class Crop
    {
        public float Maturity { get; set; } = 0;
        public float GrowthRate { get; set; } = 0.1f;

        public ResourceData Resource { get; set; }
        
        public Crop(ResourceData resource)
        {
            Resource = resource;
        }
        
        public void Grow(float growthEfficiency)
        {
            Debug.Log("Crop growing: " + Maturity);
            if (Maturity < 1) Maturity += growthEfficiency * GrowthRate;
        }

        public ResourceData GiveResource()
        {
            return Resource;
        }
    }
}