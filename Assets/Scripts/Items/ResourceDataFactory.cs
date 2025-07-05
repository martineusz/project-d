using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    public class ResourceDataFactory : MonoBehaviour
    {
        [SerializeField] private string defaultResourceName = "unnamed";
        [SerializeField] private Sprite defaultIcon = null;

        public ResourceData GenerateNewResource()
        {
            ResourceData resource = ScriptableObject.CreateInstance<ResourceData>();
            resource.itemName = defaultResourceName;
            resource.icon = defaultIcon;
            return resource;
        }
    }
}