using Environment;
using UI;
using UnityEngine;

namespace Shop
{
    public class ShopPlace : MonoBehaviour, INteractive
    {
        public ShopUI shopUI;

        public void Use()
        {
            shopUI.OpenShop();
        }
    }
}