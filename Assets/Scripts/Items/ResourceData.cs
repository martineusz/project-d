using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "newResource", menuName = "Items/Resource", order = 0)]
    public class ResourceData : ItemData
    {
        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}