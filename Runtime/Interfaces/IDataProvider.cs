namespace ED.DataManagement.Interfaces
{
    public interface IDataProvider
    {
        string LoadData(string name);
        void SaveData(string name, string data);
    }
}