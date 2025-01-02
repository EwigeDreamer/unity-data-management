namespace ED.PrefsDataManagement.Interfaces
{
    public interface IPrefsDataProvider
    {
        string LoadData(string name);
        void SaveData(string name, string data);
    }
}