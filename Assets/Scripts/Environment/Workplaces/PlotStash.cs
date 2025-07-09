using UI.Inventory;
using UnityEngine;

namespace Environment.Workplaces
{
    public class PlotStash : MonoBehaviour, INteractive
    {
        public WildPlot plot;
        public void Use()
        {
            if (!plot) return;
            if (plot.TryTakeResource(out var resource))
            {
                Inventory.Instance.Add(resource);
                Debug.Log("Resource taken from plot and added to inventory.");
            }
            else
            {
                Debug.Log("No resources in plot stash.");
            }
        }
    }
}