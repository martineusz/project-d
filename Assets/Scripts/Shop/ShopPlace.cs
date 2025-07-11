using Environment;
using UI;
using UI.Shop;
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