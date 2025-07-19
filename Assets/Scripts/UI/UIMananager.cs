using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        public bool IsAnyUIOpen { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        public void SetUIOpen(bool isOpen)
        {
            IsAnyUIOpen = isOpen;
        }
    }
}