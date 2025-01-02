using System.Collections;
using ED.PrefsDataManagement.Logic;
using UnityEngine;

namespace ED.PrefsDataManagement.Samples
{
    public class DataManagerEntryPoint : MonoBehaviour
    {
        private const string LabelText = "If you have Sirenix Odin Inspector, open DataManager window in " + Constants.EditorButtonPath;
        
        public static DataManagerEntryPoint Instance { get; private set; }
        public PrefsDataManager DataManager { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            DataManager = new PrefsDataManager(new UnityPrefsDataProvider());
        }

        private IEnumerator Start()
        {
            var wait = new WaitForSeconds(1f);
            while (true)
            {
                yield return wait;
                var data = DataManager.GetData<PrefsDataExample1>();
                data.SetRandom();
                data.Save();
            }
        }

        private void OnGUI()
        {
            var rect = new Rect(10, 10, Screen.width - 20, Screen.height - 20);
            GUI.Label(rect, LabelText);
        }

        private void OnDestroy()
        {
            DataManager?.Dispose();
            DataManager = null;
            
            Instance = null;
        }
    }
}