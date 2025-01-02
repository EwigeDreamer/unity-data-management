using System.Collections.Generic;

namespace ED.DataManagement
{
    public interface IDataManager
    {
        List<BaseData> GetAllData();
        T GetData<T>() where T : BaseData, new();
        void SaveAll();
    }
}