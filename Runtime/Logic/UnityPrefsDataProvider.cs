using ED.PrefsDataManagement.Interfaces;
using UnityEngine;

namespace ED.PrefsDataManagement.Logic
{
    public class UnityPrefsDataProvider : IPrefsDataProvider
    {
        public string LoadData(string name)
        {
            if (!PlayerPrefs.HasKey(name)) return string.Empty;
            return PlayerPrefs.GetString(name, string.Empty);
        }

        public void SaveData(string name, string data)
        {
            PlayerPrefs.SetString(name, data);
        }
    }
}