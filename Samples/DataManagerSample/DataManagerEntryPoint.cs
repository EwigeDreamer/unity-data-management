using UnityEngine;

namespace ED.DataManagement.Samples
{
    public class DataManagerEntryPoint : MonoBehaviour
    {
        public static DataManagerEntryPoint Instance { get; private set; }
        public DataManager DataManager { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            DataManager = new DataManager(new PrefsDataProvider());
        }

        private void OnDestroy()
        {
            DataManager?.Dispose();
            DataManager = null;
            
            Instance = null;
        }
    }
}