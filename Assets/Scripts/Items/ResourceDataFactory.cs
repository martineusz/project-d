using UnityEngine;

namespace Items
{
    public class ResourceDataFactory
    {
        private string _defaultResourceName = "unnamed";
        private Sprite _defaultIcon = null;
        public ResourceData GenerateNewResource()
        {
            ResourceData resource = ScriptableObject.CreateInstance<ResourceData>();
            resource.itemName = _defaultResourceName;
            resource.icon = _defaultIcon;
            
            return resource;
        }
    }
}