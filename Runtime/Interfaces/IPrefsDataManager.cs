using System.Collections.Generic;
using ED.PrefsDataManagement.Base;

namespace ED.PrefsDataManagement.Interfaces
{
    public interface IPrefsDataManager
    {
        List<BasePrefsData> GetAllData();
        T GetData<T>() where T : BasePrefsData, new();
        void SaveAll();
    }
}