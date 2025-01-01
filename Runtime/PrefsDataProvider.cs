using UnityEngine;

namespace ED.DataManagement
{
    public class PrefsDataProvider : IDataProvider
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