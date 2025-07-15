using Items;

namespace Environment.Miscellaneous
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
            if (Maturity < 1) Maturity += growthEfficiency * GrowthRate;
        }

        public ResourceData GiveResource()
        {
            return Resource;
        }
    }
}