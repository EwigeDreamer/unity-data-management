using System.Collections.Generic;
using ED.DataManagement.Base;

namespace ED.DataManagement.Interfaces
{
    public interface IDataManager
    {
        List<BaseData> GetAllData();
        T GetData<T>() where T : BaseData, new();
        void SaveAll();
    }
}