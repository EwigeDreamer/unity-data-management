using System;

namespace ED.DataManagement
{
    [System.Serializable]
    public abstract class DataBase : IDisposable
    {
        [field: NonSerialized] public event Action OnSave;
        
        public void Save() => OnSave?.Invoke();

        public void Dispose()
        {
            OnSave = null;
        }
    }
}