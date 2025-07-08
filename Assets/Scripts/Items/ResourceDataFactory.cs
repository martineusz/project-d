using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    public class ResourceDataFactory : MonoBehaviour
    {
        [SerializeField] private string defaultResourceName = "unnamed";
        [SerializeField] private Sprite defaultIcon = null;
        [SerializeField, TextArea] private string defaltDescription = "No description provided.";
        [SerializeField] Rarity defaultRarity = Rarity.Common;
        public ResourceData GenerateNewResource()
        {
            ResourceData resource = ScriptableObject.CreateInstance<ResourceData>();
            resource.itemName = defaultResourceName;
            resource.icon = defaultIcon;
            resource.description = defaltDescription;
            resource.rarity = defaultRarity;
            return resource;
        }
    }
}