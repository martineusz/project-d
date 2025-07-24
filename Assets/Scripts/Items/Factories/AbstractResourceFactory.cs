using UnityEngine;

namespace Items.Factories
{
    public abstract class AbstractResourceFactory : MonoBehaviour
    {
        public abstract ResourceData GenerateNewResource();
    }
}